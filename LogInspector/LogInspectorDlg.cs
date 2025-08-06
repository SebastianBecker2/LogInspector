namespace LogInspector
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using LogInspector.Properties;
    using Newtonsoft.Json.Linq;
    using Serilog.Events;
    using Serilog.Parsing;
    using static Vanara.PInvoke.User32;

    public partial class LogInspectorDlg : Form
    {

        private List<CachedLogEvent> filteredLogEvents = [];
        private List<CachedLogEvent> logEvents = [];
        private bool updatingFilters;
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

            Shown += (s, e) => LoadLogFilesToolStripMenuItem.PerformClick();
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
                Filter = "Log Files (*.log)|*.log|All Files (*.*)|*.*",
                Multiselect = true,
            };

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            logEvents = [.. dlg.FileNames
                .Select(ParseLogFile)
                .SelectMany(logEvent => logEvent)];

            UpdateFilters(logEvents);
            UpdateLogEvents();
        }

        private static IEnumerable<CachedLogEvent> ParseLogFile(string file)
        {
            var parser = new MessageTemplateParser();

            using var stream = new FileStream(
                file,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line is null)
                {
                    continue;
                }
                var jObject = JObject.Parse(line);

                var timestamp = jObject["Timestamp"]!.ToObject<DateTimeOffset>();
                var level = Enum.Parse<LogEventLevel>(jObject["Level"]!.ToString());
                var template = jObject["MessageTemplate"]!.ToString();
                var messageTemplate = parser.Parse(template);
                var exception = jObject["Exception"]?.ToString();

                var properties = ParseProperties(jObject);

                var logEvent = new LogEvent(
                        timestamp,
                        level,
                        exception: null,
                        messageTemplate,
                        properties.ConvertAll(p => p.Value)
                      );
                yield return new CachedLogEvent(logEvent, exception);
            }
        }

        private static List<KeyValuePair<string, LogEventProperty>> ParseProperties(
            JObject jObject)
        {
            static ScalarValue ToScalarValue(JToken value) =>
                new(value.Type switch
                {
                    JTokenType.Integer => value.ToObject<int>(),
                    JTokenType.Float => value.ToObject<double>(),
                    JTokenType.String => value.ToObject<string>(),
                    JTokenType.Boolean => value.ToObject<bool>(),
                    JTokenType.None => throw new NotImplementedException(),
                    JTokenType.Object => throw new NotImplementedException(),
                    JTokenType.Array => throw new NotImplementedException(),
                    JTokenType.Constructor => throw new NotImplementedException(),
                    JTokenType.Property => throw new NotImplementedException(),
                    JTokenType.Comment => throw new NotImplementedException(),
                    JTokenType.Null => throw new NotImplementedException(),
                    JTokenType.Undefined => throw new NotImplementedException(),
                    JTokenType.Date => throw new NotImplementedException(),
                    JTokenType.Raw => throw new NotImplementedException(),
                    JTokenType.Bytes => throw new NotImplementedException(),
                    JTokenType.Guid => throw new NotImplementedException(),
                    JTokenType.Uri => throw new NotImplementedException(),
                    JTokenType.TimeSpan => throw new NotImplementedException(),
                    _ => value.ToString()
                });

            if (jObject["Properties"] == null)
            {
                return [];
            }

            return [.. jObject["Properties"]!
                .Children<JProperty>()
                .Select(prop => new KeyValuePair<string, LogEventProperty>(
                    prop.Name,
                    new LogEventProperty(
                        prop.Name,
                        ToScalarValue(prop.Value))
                ))];
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
            else if (key == "Message")
            {
                e.Value = logEvent.Message;
                return;
            }
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
                CblLevel.Items.AddRange([.. logEvents
                    .Select(logEvent => logEvent.Level)
                    .Distinct()
                    .Cast<object>()]);
                ShowHorizontalScrollbar(CblLevel);
                AutoSizeHeight(CblLevel);
                CheckAll(CblLevel);

                CblMessageTemplate.Items.Clear();
                CblMessageTemplate.Items.AddRange([.. logEvents
                    .Select(logEvent => logEvent.MessageTemplate.Text)
                    .Distinct()
                    .Cast<object>()]);
                ShowHorizontalScrollbar(CblMessageTemplate);
                AutoSizeHeight(CblMessageTemplate);
                CheckAll(CblMessageTemplate);

                CblExceptions.Items.Clear();
                CblExceptions.Items.AddRange([.. logEvents
                    .Where(logEvent => logEvent.ExceptionType is not null)
                    .Select(logEvent => logEvent.ExceptionType!)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToArray()]);
                ShowHorizontalScrollbar(CblExceptions);
                AutoSizeHeight(CblExceptions);
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

            foreach (var prop in logEvents
                .SelectMany(logEvent => logEvent.Properties)
                .DistinctBy(p => new { p.Key, p.Value }))
            {
                if (!KnownProperties.TryGetValue(prop.Key, out var cbl))
                {
                    _ = TlpProperties.ColumnStyles.Add(
                        new ColumnStyle(SizeType.AutoSize));
                    TlpProperties.Controls.Add(
                        new Label
                        {
                            Text = prop.Key,
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

                    KnownProperties.Add(prop.Key, cbl);
                }

                var val = CachedLogEvent.ToString(prop.Value);
                _ = cbl.Items.Add(val);
            }

            foreach (var cbl in TlpProperties.Controls
                .Cast<Control>()
                .Select(c => c as CheckBoxList)
                .Where(c => c is not null))
            {
                AutoSizeWidth(cbl!);
                ShowHorizontalScrollbar(cbl!);
                AutoSizeHeight(cbl!);
                CheckAll(cbl!);
            }
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

            var t = Stopwatch.StartNew();
            var minDate = DateTime
                .Parse(CmbStartDate.Text, CultureInfo.InvariantCulture)
                .Add(DtpStartTime.Value.TimeOfDay);
            var maxDate = DateTime
                .Parse(CmbEndDate.Text, CultureInfo.InvariantCulture)
                .Add(DtpEndTime.Value.TimeOfDay);
            var selectedLevels = CblLevel.Tag as HashSet<string> ?? [];
            var selectedTemplates = CblMessageTemplate.Tag as HashSet<string> ?? [];
            var filtered = logEvents
                .Where(logEvent =>
                    logEvent.Timestamp >= minDate
                    && logEvent.Timestamp <= maxDate)
                .Where(logEvent =>
                    selectedLevels.Count == CblLevel.Items.Count
                    || selectedLevels.Contains($"{logEvent.Level}"))
                .Where(logEvent =>
                    selectedTemplates.Count == CblMessageTemplate.Items.Count
                    || selectedTemplates.Contains($"{logEvent.MessageTemplate}"))
                .Where(logEvent =>
                    logEvent.Message.Contains(
                        TxtMessage.Text,
                        StringComparison.OrdinalIgnoreCase));

            foreach (var (name, cbl) in KnownProperties)
            {
                var values = cbl.Tag as HashSet<string> ?? [];
                filtered = filtered.Where(logEvents =>
                    values.Count == cbl.Items.Count
                    || !logEvents.Properties.TryGetValue(name, out var value)
                    || values.Contains(CachedLogEvent.ToString(value)));
            }

            filteredLogEvents = [.. filtered];

            DgvLogEvents.SuspendLayout();
            DgvLogEvents.Rows.Clear();
            DgvLogEvents.RowCount = filteredLogEvents.Count;
            DgvLogEvents.ResumeLayout();

            LblEventCount.Text = $"Filtered {logEvents.Count} log events to " +
                $"{filteredLogEvents.Count} in {t.ElapsedMilliseconds} ms";
        }

        private static bool IsShowingHorizontalScrollbar(CheckedListBox clb)
        {
            var sbInfo = new SCROLLBARINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(SCROLLBARINFO))
            };

            if (!GetScrollBarInfo(
                clb.Handle,
                ObjectIdentifiers.OBJID_HSCROLL,
                ref sbInfo))
            {
                return false;
            }

            return (sbInfo.rgstate[(int)SB.SB_HORZ]
                & (uint)ComboBoxInfoState.STATE_SYSTEM_INVISIBLE)
                == 0;
        }

        private static void AutoSizeHeight(CheckedListBox clb)
        {
            var countOffset = IsShowingHorizontalScrollbar(clb) ? 1 : 0;
            clb.Height = (clb.ItemHeight
                * Math.Max(clb.Items.Count + countOffset, 1))
                + 4;
        }

        private static void AutoSizeWidth(CheckedListBox clb)
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
                return;
            }

            widths.Sort(); // Ascending

            var index = (int)(percentile * widths.Count);
            var chosenWidth = widths[Math.Min(index, widths.Count - 1)];

            // Add scrollbar + padding
            clb.Width = (int)chosenWidth
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
            clb!.HorizontalScrollbar = true;
            clb.IntegralHeight = false;

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

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using var dlg = new AboutBox1();
            _ = dlg.ShowDialog(this);
        }
    }
}
