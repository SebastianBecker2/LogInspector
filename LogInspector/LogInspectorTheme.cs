namespace LogInspector;

using System.Windows.Forms;

internal static class LogInspectorTheme
{
    private static readonly DarkToolStripColorTable DarkColorTable = new();
    private static readonly ToolStripProfessionalRenderer DarkMenuRenderer = new(DarkColorTable);
    private static readonly ToolStripProfessionalRenderer LightMenuRenderer = new();

    public static bool IsDarkMode { get; private set; } = true;

    public static readonly Color DarkBackground = Color.FromArgb(30, 30, 30);
    public static readonly Color DarkSurface = Color.FromArgb(37, 37, 38);
    public static readonly Color DarkControl = Color.FromArgb(45, 45, 48);
    public static readonly Color DarkBorder = Color.FromArgb(63, 63, 70);
    public static readonly Color DarkForeground = Color.FromArgb(241, 241, 241);
    public static readonly Color DarkMuted = Color.FromArgb(180, 180, 180);
    public static readonly Color DarkSelection = Color.FromArgb(0, 122, 204);
    public static readonly Color DarkSelectionText = Color.White;
    public static readonly Color DarkGridLine = Color.FromArgb(55, 55, 58);

    public static void SetDarkMode(bool darkMode, Control root)
    {
        IsDarkMode = darkMode;
        Apply(root);
    }

    public static void Apply(Control root)
    {
        ApplyControl(root);
        foreach (Control child in root.Controls)
        {
            Apply(child);
        }
    }

    private static void ApplyControl(Control control)
    {
        switch (control)
        {
            case Form form:
                ApplyForm(form);
                break;
            case MenuStrip menuStrip:
                ApplyToolStrip(menuStrip);
                break;
            case StatusStrip statusStrip:
                ApplyToolStrip(statusStrip);
                break;
            case ContextMenuStrip contextMenu:
                ApplyContextMenu(contextMenu);
                break;
            case ToolStrip toolStrip:
                ApplyToolStrip(toolStrip);
                break;
            case SplitContainer split:
                ApplySplitContainer(split);
                break;
            case DataGridView grid:
                ApplyDataGridView(grid);
                break;
            case TableLayoutPanel:
            case Panel:
                ApplyContainer(control);
                break;
            case Label label:
                ApplyLabel(label);
                break;
            case TextBox textBox:
                ApplyTextBox(textBox);
                break;
            case ComboBox comboBox:
                ApplyComboBox(comboBox);
                break;
            case DateTimePicker dateTimePicker:
                ApplyDateTimePicker(dateTimePicker);
                break;
            case CheckedListBox checkedListBox:
                ApplyCheckedListBox(checkedListBox);
                break;
            case ListBox listBox:
                ApplyListBox(listBox);
                break;
            case Button button:
                ApplyButton(button);
                break;
        }
    }

    private static void ApplyForm(Form form)
    {
        if (IsDarkMode)
        {
            form.BackColor = DarkBackground;
            form.ForeColor = DarkForeground;
        }
        else
        {
            form.BackColor = SystemColors.Control;
            form.ForeColor = SystemColors.ControlText;
        }
    }

    private static void ApplyContainer(Control control)
    {
        if (IsDarkMode)
        {
            control.BackColor = DarkSurface;
            control.ForeColor = DarkForeground;
        }
        else
        {
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;
        }
    }

    private static void ApplyLabel(Label label)
    {
        if (IsDarkMode)
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = DarkForeground;
        }
        else
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = SystemColors.ControlText;
        }
    }

    private static void ApplyTextBox(TextBox textBox)
    {
        if (IsDarkMode)
        {
            textBox.BackColor = DarkControl;
            textBox.ForeColor = DarkForeground;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }
        else
        {
            textBox.BackColor = SystemColors.Window;
            textBox.ForeColor = SystemColors.WindowText;
            textBox.BorderStyle = BorderStyle.Fixed3D;
        }
    }

    private static void ApplyComboBox(ComboBox comboBox)
    {
        if (IsDarkMode)
        {
            comboBox.BackColor = DarkControl;
            comboBox.ForeColor = DarkForeground;
            comboBox.FlatStyle = FlatStyle.Flat;
        }
        else
        {
            comboBox.BackColor = SystemColors.Window;
            comboBox.ForeColor = SystemColors.WindowText;
            comboBox.FlatStyle = FlatStyle.Standard;
        }
    }

    private static void ApplyDateTimePicker(DateTimePicker dateTimePicker)
    {
        if (IsDarkMode)
        {
            dateTimePicker.BackColor = DarkControl;
            dateTimePicker.ForeColor = DarkForeground;
            dateTimePicker.CalendarMonthBackground = DarkControl;
            dateTimePicker.CalendarForeColor = DarkForeground;
            dateTimePicker.CalendarTitleBackColor = DarkSurface;
            dateTimePicker.CalendarTitleForeColor = DarkForeground;
            dateTimePicker.CalendarTrailingForeColor = DarkMuted;
        }
        else
        {
            dateTimePicker.BackColor = SystemColors.Window;
            dateTimePicker.ForeColor = SystemColors.WindowText;
            dateTimePicker.CalendarMonthBackground = SystemColors.Window;
            dateTimePicker.CalendarForeColor = SystemColors.WindowText;
            dateTimePicker.CalendarTitleBackColor = SystemColors.ActiveCaption;
            dateTimePicker.CalendarTitleForeColor = SystemColors.ActiveCaptionText;
            dateTimePicker.CalendarTrailingForeColor = SystemColors.GrayText;
        }
    }

    private static void ApplyCheckedListBox(CheckedListBox list)
    {
        if (IsDarkMode)
        {
            list.BackColor = DarkControl;
            list.ForeColor = DarkForeground;
            list.BorderStyle = BorderStyle.FixedSingle;
        }
        else
        {
            list.BackColor = SystemColors.Window;
            list.ForeColor = SystemColors.WindowText;
            list.BorderStyle = BorderStyle.Fixed3D;
        }
    }

    private static void ApplyListBox(ListBox list)
    {
        if (IsDarkMode)
        {
            list.BackColor = DarkControl;
            list.ForeColor = DarkForeground;
            list.BorderStyle = BorderStyle.FixedSingle;
        }
        else
        {
            list.BackColor = SystemColors.Window;
            list.ForeColor = SystemColors.WindowText;
            list.BorderStyle = BorderStyle.Fixed3D;
        }
    }

    private static void ApplyButton(Button button)
    {
        if (IsDarkMode)
        {
            button.BackColor = DarkControl;
            button.ForeColor = DarkForeground;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = DarkBorder;
            button.UseVisualStyleBackColor = false;
        }
        else
        {
            button.UseVisualStyleBackColor = true;
            button.FlatStyle = FlatStyle.Standard;
        }
    }

    private static void ApplySplitContainer(SplitContainer split)
    {
        if (IsDarkMode)
        {
            split.BackColor = DarkBorder;
            split.Panel1.BackColor = DarkSurface;
            split.Panel2.BackColor = DarkBackground;
        }
        else
        {
            split.BackColor = SystemColors.Control;
            split.Panel1.BackColor = SystemColors.Control;
            split.Panel2.BackColor = SystemColors.Control;
        }
    }

    private static void ApplyToolStrip(ToolStrip toolStrip)
    {
        toolStrip.RenderMode = ToolStripRenderMode.Professional;
        if (IsDarkMode)
        {
            toolStrip.Renderer = DarkMenuRenderer;
            toolStrip.BackColor = DarkSurface;
            toolStrip.ForeColor = DarkForeground;
        }
        else
        {
            toolStrip.Renderer = LightMenuRenderer;
            toolStrip.BackColor = SystemColors.Control;
            toolStrip.ForeColor = SystemColors.ControlText;
        }

        foreach (ToolStripItem item in toolStrip.Items)
        {
            ApplyToolStripItem(item);
        }

        toolStrip.Invalidate(true);
    }

    private static void ApplyContextMenu(ContextMenuStrip menu)
    {
        menu.RenderMode = ToolStripRenderMode.Professional;
        if (IsDarkMode)
        {
            menu.Renderer = DarkMenuRenderer;
            menu.BackColor = DarkControl;
            menu.ForeColor = DarkForeground;
        }
        else
        {
            menu.Renderer = LightMenuRenderer;
            menu.BackColor = SystemColors.Menu;
            menu.ForeColor = SystemColors.MenuText;
        }

        foreach (ToolStripItem item in menu.Items)
        {
            ApplyToolStripItem(item);
        }
    }

    private static void ApplyToolStripItem(ToolStripItem item)
    {
        if (IsDarkMode)
        {
            item.ForeColor = DarkForeground;
            item.BackColor = Color.Empty;
        }
        else
        {
            item.ForeColor = Color.Empty;
            item.BackColor = Color.Empty;
        }

        if (item is ToolStripDropDownItem dropDown)
        {
            ApplyToolStripDropDown(dropDown.DropDown);
            foreach (ToolStripItem child in dropDown.DropDownItems)
            {
                ApplyToolStripItem(child);
            }
        }
    }

    private static void ApplyToolStripDropDown(ToolStripDropDown dropDown)
    {
        dropDown.RenderMode = ToolStripRenderMode.Professional;
        if (IsDarkMode)
        {
            dropDown.Renderer = DarkMenuRenderer;
            dropDown.BackColor = DarkControl;
            dropDown.ForeColor = DarkForeground;
        }
        else
        {
            dropDown.Renderer = LightMenuRenderer;
            dropDown.BackColor = SystemColors.Menu;
            dropDown.ForeColor = SystemColors.MenuText;
        }
    }

    private sealed class DarkToolStripColorTable : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin => DarkSurface;
        public override Color ToolStripGradientMiddle => DarkSurface;
        public override Color ToolStripGradientEnd => DarkSurface;
        public override Color ToolStripBorder => DarkBorder;
        public override Color ToolStripDropDownBackground => DarkControl;
        public override Color MenuBorder => DarkBorder;
        public override Color MenuItemBorder => DarkBorder;
        public override Color MenuItemSelected => DarkSelection;
        public override Color MenuItemSelectedGradientBegin => DarkSelection;
        public override Color MenuItemSelectedGradientEnd => DarkSelection;
        public override Color MenuItemPressedGradientBegin => DarkControl;
        public override Color MenuItemPressedGradientMiddle => DarkControl;
        public override Color MenuItemPressedGradientEnd => DarkControl;
        public override Color ImageMarginGradientBegin => DarkControl;
        public override Color ImageMarginGradientMiddle => DarkControl;
        public override Color ImageMarginGradientEnd => DarkControl;
        public override Color SeparatorDark => DarkBorder;
        public override Color SeparatorLight => DarkBorder;
        public override Color StatusStripGradientBegin => DarkSurface;
        public override Color StatusStripGradientEnd => DarkSurface;
        public override Color OverflowButtonGradientBegin => DarkControl;
        public override Color OverflowButtonGradientMiddle => DarkControl;
        public override Color OverflowButtonGradientEnd => DarkControl;
    }

    private static void ApplyDataGridView(DataGridView grid)
    {
        grid.EnableHeadersVisualStyles = false;

        if (IsDarkMode)
        {
            grid.BackgroundColor = DarkBackground;
            grid.GridColor = DarkGridLine;
            grid.BorderStyle = BorderStyle.None;

            var cell = new DataGridViewCellStyle
            {
                BackColor = DarkBackground,
                ForeColor = DarkForeground,
                SelectionBackColor = DarkSelection,
                SelectionForeColor = DarkSelectionText,
            };
            grid.DefaultCellStyle = cell;
            grid.RowsDefaultCellStyle = cell;
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(cell)
            {
                BackColor = DarkSurface,
            };

            var header = new DataGridViewCellStyle
            {
                BackColor = DarkControl,
                ForeColor = DarkForeground,
                SelectionBackColor = DarkControl,
                SelectionForeColor = DarkForeground,
            };
            grid.ColumnHeadersDefaultCellStyle = header;
            grid.RowHeadersDefaultCellStyle = header;
        }
        else
        {
            grid.BackgroundColor = SystemColors.Window;
            grid.GridColor = SystemColors.ControlDark;
            grid.BorderStyle = BorderStyle.Fixed3D;

            var cell = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = SystemColors.ControlText,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
            };
            grid.DefaultCellStyle = cell;
            grid.RowsDefaultCellStyle = cell;
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(cell)
            {
                BackColor = SystemColors.ControlLight,
            };

            var header = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
            };
            grid.ColumnHeadersDefaultCellStyle = header;
            grid.RowHeadersDefaultCellStyle = header;
        }
    }
}
