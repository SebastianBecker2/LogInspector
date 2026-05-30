namespace LogInspector;

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

public partial class LogInspectorDlg : Form
{
    private ToolStripMenuItem? _darkModeMenuItem;

    private List<CachedLogEvent> filteredLogEvents = [];
    private List<CachedLogEvent> logEvents = [];
    private string[] loadedFiles = [];
    private bool updatingFilters;
    private readonly Dictionary<string, Dictionary<string, List<int>>> _filterIndex = [];
    private static readonly Dictionary<string, CheckBoxList> KnownProperties = [];

    public LogInspectorDlg()
    {
        InitializeComponent();

        void HandleItemCheck(
            object? sender,
            ItemCheckEventArgs e)
        {
            var cbl = sender as CheckedListBox
                ?? throw new InvalidOperationException("Invalid sender");
            UpdateCheckedItems(cbl, e);
            UpdateLogEvents();
        }

        CmbStartDate.SelectedIndexChanged += (s, e) => UpdateLogEvents();
        DtpStartTime.ValueChanged += (s, e) => UpdateLogEvents();
        CmbEndDate.SelectedIndexChanged += (s, e) => UpdateLogEvents();
        DtpEndTime.ValueChanged += (s, e) => UpdateLogEvents();
        CblLevel.ItemCheck += HandleItemCheck;
        CblMessageTemplate.ItemCheck += HandleItemCheck;
        TxtMessage.TextChanged += (s, e) => UpdateLogEvents();

        ConfigureStaticFilterLists();
        ConfigureLogEventsGrid();
        ConfigureThemeMenu();

        Shown += (s, e) => LoadLogFilesToolStripMenuItem.PerformClick();
    }

    private void ConfigureThemeMenu()
    {
        viewToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
        _darkModeMenuItem = new ToolStripMenuItem("Dark Mode")
        {
            CheckOnClick = true,
            Checked = LogInspectorTheme.IsDarkMode,
        };
        _darkModeMenuItem.Click += (_, _) =>
            LogInspectorTheme.SetDarkMode(_darkModeMenuItem.Checked, this);
        viewToolStripMenuItem.DropDownItems.Add(_darkModeMenuItem);
        LogInspectorTheme.Apply(this);
    }

    private void ConfigureLogEventsGrid()
    {
        var copyMenuItem = new ToolStripMenuItem(
            "Copy to Clipboard",
            null,
            (_, _) => CopySelectedLogEventsToClipboard());
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(copyMenuItem);
        contextMenu.Opening += (_, e) =>
        {
            if (DgvLogEvents.SelectedRows.Count == 0)
            {
                e.Cancel = true;
            }
        };
        DgvLogEvents.ContextMenuStrip = contextMenu;
        LogInspectorTheme.Apply(contextMenu);
    }

    private void CopySelectedLogEventsToClipboard()
    {
        var selected = CaptureSelectedLogEvents();

        if (selected.Count == 0)
        {
            return;
        }

        Clipboard.SetText(LogEventJson.SerializeMany(selected));
    }

    private List<CachedLogEvent> CaptureSelectedLogEvents()
    {
        if (filteredLogEvents.Count == 0 || DgvLogEvents.SelectedRows.Count == 0)
        {
            return [];
        }

        return DgvLogEvents.SelectedRows
            .Cast<DataGridViewRow>()
            .Select(row => row.Index)
            .Where(index => index >= 0 && index < filteredLogEvents.Count)
            .OrderBy(index => index)
            .Select(index => filteredLogEvents[index])
            .ToList();
    }

    private void RestoreSelectionAndScroll(List<CachedLogEvent> previouslySelected)
    {
        if (previouslySelected.Count == 0 || filteredLogEvents.Count == 0)
        {
            return;
        }

        var indicesToSelect = new List<int>();
        foreach (var selected in previouslySelected)
        {
            var index = filteredLogEvents.IndexOf(selected);
            if (index >= 0)
            {
                indicesToSelect.Add(index);
            }
        }

        if (indicesToSelect.Count == 0)
        {
            return;
        }

        DgvLogEvents.ClearSelection();
        foreach (var index in indicesToSelect)
        {
            DgvLogEvents.Rows[index].Selected = true;
        }

        DgvLogEvents.FirstDisplayedScrollingRowIndex = indicesToSelect[0];
    }

    private void ConfigureStaticFilterLists()
    {
        ApplyFilterListLayout(CblLevel);
        ApplyFilterListLayout(CblMessageTemplate);
        ApplyFilterListLayout(CblExceptions);
    }
    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) =>
        Application.Exit();

    private void LoadLogFilesToolStripMenuItem_Click(
        object sender,
        EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select Log Files",
            Filter = "Log Files (*.log;*.xml)|*.log;*.xml|All Files (*.*)|*.*",
            Multiselect = true,
        };

        if (dlg.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        loadedFiles = dlg.FileNames;
        ReloadCurrentLogFiles();
    }

    private void ReloadCurrentLogFilesToolStripMenuItem_Click(
        object sender,
        EventArgs e)
    {
        if (loadedFiles.Length == 0)
        {
            LoadLogFilesToolStripMenuItem.PerformClick();
            return;
        }

        ReloadCurrentLogFiles();
    }

    private void DgvLogEvents_CellValueNeeded(
        object sender,
        DataGridViewCellValueEventArgs e)
    {
        if (filteredLogEvents is null
            || e.RowIndex >= filteredLogEvents.Count)
        {
            return;
        }

        var logEvent = filteredLogEvents[e.RowIndex];
        var key = DgvLogEvents.Columns[e.ColumnIndex].HeaderText;

        if (key == "Timestamp")
        {
            e.Value = $"{logEvent.Timestamp:yyyy-MM-dd HH:mm:ss.fff}";
            return;
        }
        else if (key == "Level")
        {
            e.Value = logEvent.Level;
            return;
        }
        else if (key == "Logger")
        {
            e.Value = logEvent.LoggerName;
            return;
        }
        else if (key == "Message")
        {
            e.Value = logEvent.Message;
            return;
        }
    }

    private void ReloadCurrentLogFiles()
    {
        logEvents = [.. loadedFiles
            .SelectMany(LogFileParser.ParseLogFile)
            .OrderByDescending(logEvent => logEvent.Timestamp)];
        BuildFilterIndex();
        UpdateFilters(logEvents);
        UpdateLogEvents();
    }

    private void BuildFilterIndex()
    {
        _filterIndex.Clear();

        for (var i = 0; i < logEvents.Count; i++)
        {
            var logEvent = logEvents[i];
            AddToIndex("Level", $"{logEvent.Level}", i);

            if (logEvent.HasMessageTemplate)
            {
                AddToIndex("MessageTemplate", $"{logEvent.MessageTemplate}", i);
            }

            if (!string.IsNullOrEmpty(logEvent.ExceptionType))
            {
                AddToIndex("Exception", logEvent.ExceptionType!, i);
            }

            if (!string.IsNullOrEmpty(logEvent.LoggerName))
            {
                AddToIndex("Logger", logEvent.LoggerName, i);
            }

            foreach (var (key, value) in logEvent.Properties)
            {
                var normalizedKey = NormalizeFilterPropertyName(key);
                if (normalizedKey == "Logger")
                {
                    continue;
                }

                AddToIndex(normalizedKey, CachedLogEvent.ToString(value), i);
            }
        }
    }

    private void AddToIndex(string key, string value, int index)
    {
        if (!_filterIndex.TryGetValue(key, out var valueMap))
        {
            valueMap = new Dictionary<string, List<int>>();
            _filterIndex[key] = valueMap;
        }

        if (!valueMap.TryGetValue(value, out var indices))
        {
            indices = [];
            valueMap[value] = indices;
        }

        indices.Add(index);
    }

    private void UpdateFilters(IEnumerable<CachedLogEvent> logEvents)
    {
        updatingFilters = true;

        try
        {
            var dates = logEvents
                .Select(logEvent => logEvent.Timestamp.Date)
                .Distinct()
                .OrderBy(date => date)
                .Select(date => $"{date:yyyy-MM-dd}")
                .ToArray();
            CmbStartDate.Items.Clear();
            CmbStartDate.Items.AddRange(dates);
            CmbStartDate.SelectedIndex = 0;
            CmbEndDate.Items.Clear();
            CmbEndDate.Items.AddRange(dates);
            CmbEndDate.SelectedIndex = dates.Length - 1;

            // First set MaxDate to set MinDate freely.
            DtpStartTime.MaxDate = DateTimePicker.MaximumDateTime;
            DtpStartTime.MinDate = logEvents
                .Min(logEvent => logEvent.Timestamp.DateTime);
            DtpStartTime.MaxDate = DtpStartTime.MinDate.Date
                .AddDays(1)
                .AddTicks(-1);
            DtpStartTime.Value = DtpStartTime.MinDate;

            // First reset MinDate to set MaxDate freely.
            DtpEndTime.MinDate = DateTimePicker.MinimumDateTime;
            DtpEndTime.MaxDate = logEvents
                .Max(logEvent => logEvent.Timestamp.DateTime);
            DtpEndTime.MinDate = DtpStartTime.MaxDate.Date;
            DtpEndTime.Value = DtpEndTime.MaxDate;

            CblLevel.Items.Clear();
            CblLevel.Items.AddRange([.. (_filterIndex.TryGetValue("Level", out var levelValues)
                ? levelValues.Keys
                : Enumerable.Empty<string>())
                .Cast<object>()]);
            ApplyFilterListLayout(CblLevel);
            CheckAll(CblLevel);

            CblMessageTemplate.Items.Clear();
            CblMessageTemplate.Items.AddRange([.. (_filterIndex.TryGetValue("MessageTemplate", out var templateValues)
                ? templateValues.Keys
                : Enumerable.Empty<string>())
                .Cast<object>()]);
            ApplyFilterListLayout(CblMessageTemplate);
            CheckAll(CblMessageTemplate);
            var showMessageTemplate = CblMessageTemplate.Items.Count > 0;
            CblMessageTemplate.Visible = showMessageTemplate;
            label4.Visible = showMessageTemplate;

            CblExceptions.Items.Clear();
            CblExceptions.Items.AddRange([.. (_filterIndex.TryGetValue("Exception", out var exceptionValues)
                ? exceptionValues.Keys.OrderBy(name => name).ToArray()
                : [])]);
            ApplyFilterListLayout(CblExceptions);
            CheckAll(CblExceptions);
            CblExceptions.Visible = CblExceptions.Items.Count > 0;
            LblExceptions.Visible = CblExceptions.Visible;

            UpdatePropertyFilters(logEvents);
        }
        finally
        {
            updatingFilters = false;
        }
    }

    private void UpdatePropertyFilters(IEnumerable<CachedLogEvent> logEvents)
    {
        TlpProperties.Controls.Clear();
        TlpProperties.ColumnCount = 0;
        TlpProperties.ColumnStyles.Clear();
        foreach (var ctrl in KnownProperties.Values)
        {
            ctrl.Dispose();
        }
        KnownProperties.Clear();

        foreach (var (propName, values) in _filterIndex
            .Where(kvp => kvp.Key is not "Level" and not "MessageTemplate" and not "Exception")
            .OrderBy(kvp => kvp.Key))
        {
            if (!KnownProperties.TryGetValue(propName, out var cbl))
            {
                _ = TlpProperties.ColumnStyles.Add(
                    new ColumnStyle(SizeType.AutoSize));
                TlpProperties.Controls.Add(
                    new Label
                    {
                        Text = propName,
                    },
                    KnownProperties.Count,
                    0);
                cbl = new CheckBoxList
                {
                    CheckOnClick = true,
                };
                cbl.ItemCheck += (s, e) =>
                {
                    UpdateCheckedItems(cbl, e);
                    UpdateLogEvents();
                };
                TlpProperties.Controls.Add(cbl, KnownProperties.Count, 1);

                KnownProperties.Add(propName, cbl);
            }

            cbl.Items.Clear();
            cbl.Items.AddRange([.. values.Keys.Cast<object>()]);
        }

        foreach (var cbl in TlpProperties.Controls
            .Cast<Control>()
            .Select(c => c as CheckBoxList)
            .Where(c => c is not null))
        {
            ApplyFilterListLayout(cbl!);
            CheckAll(cbl!);
        }

        TlpProperties.PerformLayout();
        TlpFilters.PerformLayout();
        LogInspectorTheme.Apply(TlpProperties);
    }

    private void UpdateLogEvents()
    {
        if (updatingFilters)
        {
            return;
        }
        if (logEvents.Count == 0)
        {
            DgvLogEvents.Rows.Clear();
            LblEventCount.Text = "No log events to display.";
            return;
        }

        var previouslySelected = CaptureSelectedLogEvents();

        var t = Stopwatch.StartNew();
        var minDate = DateTime
            .Parse(CmbStartDate.Text, CultureInfo.InvariantCulture)
            .Add(DtpStartTime.Value.TimeOfDay);
        var maxDate = DateTime
            .Parse(CmbEndDate.Text, CultureInfo.InvariantCulture)
            .Add(DtpEndTime.Value.TimeOfDay);
        var selectedLevels = CblLevel.Tag as HashSet<string> ?? [];
        var selectedTemplates = CblMessageTemplate.Tag as HashSet<string> ?? [];
        var messageSearch = TxtMessage.Text;
        var hasMessageSearch = !string.IsNullOrWhiteSpace(messageSearch);
        var allowedByIndex = BuildAllowedByIndex(
            selectedLevels,
            selectedTemplates);

        var filtered = new List<CachedLogEvent>(logEvents.Count);
        for (var i = 0; i < logEvents.Count; i++)
        {
            if (allowedByIndex is not null
                && !allowedByIndex[i])
            {
                continue;
            }

            var logEvent = logEvents[i];
            if (logEvent.Timestamp < minDate
                || logEvent.Timestamp > maxDate)
            {
                continue;
            }
            if (hasMessageSearch
                && !logEvent.Message.Contains(
                    messageSearch,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            filtered.Add(logEvent);
        }

        filteredLogEvents = filtered;

        DgvLogEvents.SuspendLayout();
        DgvLogEvents.RowCount = 0;
        DgvLogEvents.RowCount = filteredLogEvents.Count;
        RestoreSelectionAndScroll(previouslySelected);
        DgvLogEvents.ResumeLayout();
        DgvLogEvents.Invalidate();
        DgvLogEvents.Update();

        LblEventCount.Text = $"Filtered {logEvents.Count} log events to " +
            $"{filteredLogEvents.Count} in {t.ElapsedMilliseconds} ms";
    }

    private bool[]? BuildAllowedByIndex(HashSet<string> selectedLevels, HashSet<string> selectedTemplates)
    {
        bool[]? allowed = null;
        ApplyIndexFilter("Level", selectedLevels, CblLevel.Items.Count, ref allowed);

        if (CblMessageTemplate.Visible)
        {
            ApplyIndexFilter("MessageTemplate", selectedTemplates, CblMessageTemplate.Items.Count, ref allowed);
        }

        foreach (var (name, cbl) in KnownProperties)
        {
            var selected = cbl.Tag as HashSet<string> ?? [];
            ApplyIndexFilter(name, selected, cbl.Items.Count, ref allowed);
        }

        return allowed;
    }

    private void ApplyIndexFilter(
        string filterName,
        HashSet<string> selectedValues,
        int totalValues,
        ref bool[]? allowed)
    {
        if (totalValues == 0
            || selectedValues.Count == totalValues
            || !_filterIndex.TryGetValue(filterName, out var valueMap))
        {
            return;
        }

        var current = new bool[logEvents.Count];
        foreach (var selected in selectedValues)
        {
            if (!valueMap.TryGetValue(selected, out var indices))
            {
                continue;
            }

            foreach (var index in indices)
            {
                current[index] = true;
            }
        }

        if (allowed is null)
        {
            allowed = current;
            return;
        }

        for (var i = 0; i < allowed.Length; i++)
        {
            allowed[i] = allowed[i] && current[i];
        }
    }

    private static string NormalizeFilterPropertyName(string propertyName) =>
        propertyName is "SourceContext" or "LoggerName" or "logger"
            ? "Logger"
            : propertyName;

    private static void ApplyFilterListLayout(CheckedListBox clb)
    {
        clb.Dock = DockStyle.Fill;
        clb.IntegralHeight = false;
        clb.MinimumSize = new Size(CalculatePreferredFilterListWidth(clb), 0);
        ShowHorizontalScrollbar(clb);
    }

    private static int CalculatePreferredFilterListWidth(CheckedListBox clb)
    {
        // Ignores the longest x% of items when calculating width.
        // To avoid outliers skewing the width calculation.
        var percentile = 0.90f;

        var widths = new List<float>();
        using var g = clb.CreateGraphics();
        foreach (var item in clb.Items)
        {
            var size = g.MeasureString(item.ToString(), clb.Font);
            widths.Add(size.Width);
        }

        if (widths.Count == 0)
        {
            return 120;
        }

        widths.Sort(); // Ascending

        var index = (int)(percentile * widths.Count);
        var chosenWidth = widths[Math.Min(index, widths.Count - 1)];

        return (int)chosenWidth
            + SystemInformation.VerticalScrollBarWidth
            + 25;
    }

    private static void UpdateCheckedItems(
        CheckedListBox clb,
        ItemCheckEventArgs e)
    {
        var checkedItems = clb.Tag as HashSet<string> ?? [];
        var item = clb.Items[e.Index].ToString() ?? "";
        if (e.NewValue == CheckState.Checked)
        {
            _ = checkedItems.Add(item);
        }
        else if (e.NewValue == CheckState.Unchecked)
        {
            _ = checkedItems.Remove(item);
        }
        clb.Tag = checkedItems;
    }

    private static void CheckAll(CheckedListBox clb)
    {
        HashSet<string> checkedItems = [];
        foreach (var i in Enumerable.Range(0, clb.Items.Count))
        {
            clb.SetItemChecked(i, true);
            _ = checkedItems.Add(clb.Items[i].ToString() ?? "");
        }
        clb.Tag = checkedItems;
    }

    private static void ShowHorizontalScrollbar(CheckedListBox clb)
    {
        clb.HorizontalScrollbar = true;

        var maxItemWidth = 0;
        using var g = clb.CreateGraphics();
        foreach (var item in clb.Items)
        {
            var size = g.MeasureString(item.ToString(), clb.Font);
            if (size.Width > maxItemWidth)
            {
                maxItemWidth = (int)size.Width;
            }
        }
        clb.HorizontalExtent = maxItemWidth + 20;
    }

    private void ShowFiltersToolStripMenuItem_Click(
        object sender,
        EventArgs e) =>
        SctSplitter.Panel1Collapsed = false;

    private void HideFiltersToolStripMenuItem_Click(
        object sender,
        EventArgs e) =>
        SctSplitter.Panel1Collapsed = true;

    private void DgvLogEvents_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || filteredLogEvents.Count == 0)
        {
            return;
        }
        using var dlg = new DetailsDlg();
        dlg.LogEvent = filteredLogEvents[e.RowIndex];
        _ = dlg.ShowDialog();
    }
}
