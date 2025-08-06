namespace LogInspector
{
    partial class LogInspectorDlg
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            LoadLogFilesToolStripMenuItem = new ToolStripMenuItem();
            ExitToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            showFiltersToolStripMenuItem = new ToolStripMenuItem();
            hideFiltersToolStripMenuItem = new ToolStripMenuItem();
            DgvLogEvents = new DataGridView();
            DgcTimestamp = new DataGridViewTextBoxColumn();
            DgcLevel = new DataGridViewTextBoxColumn();
            DgcMessage = new DataGridViewTextBoxColumn();
            TlpFilters = new TableLayoutPanel();
            CblExceptions = new CheckBoxList();
            label1 = new Label();
            CmbStartDate = new ComboBox();
            label2 = new Label();
            DtpStartTime = new DateTimePicker();
            CmbEndDate = new ComboBox();
            DtpEndTime = new DateTimePicker();
            label5 = new Label();
            TlpProperties = new TableLayoutPanel();
            CblLevel = new CheckBoxList();
            CblMessageTemplate = new CheckBoxList();
            label3 = new Label();
            label4 = new Label();
            TxtMessage = new TextBox();
            LblExceptions = new Label();
            statusStrip1 = new StatusStrip();
            LblEventCount = new ToolStripStatusLabel();
            SctSplitter = new SplitContainer();
            toolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DgvLogEvents).BeginInit();
            TlpFilters.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SctSplitter).BeginInit();
            SctSplitter.Panel1.SuspendLayout();
            SctSplitter.Panel2.SuspendLayout();
            SctSplitter.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1117, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { LoadLogFilesToolStripMenuItem, ExitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // LoadLogFilesToolStripMenuItem
            // 
            LoadLogFilesToolStripMenuItem.Name = "LoadLogFilesToolStripMenuItem";
            LoadLogFilesToolStripMenuItem.Size = new Size(153, 22);
            LoadLogFilesToolStripMenuItem.Text = "Load log files...";
            LoadLogFilesToolStripMenuItem.Click += LoadLogFilesToolStripMenuItem_Click;
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            ExitToolStripMenuItem.Size = new Size(153, 22);
            ExitToolStripMenuItem.Text = "Exit";
            ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showFiltersToolStripMenuItem, hideFiltersToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // showFiltersToolStripMenuItem
            // 
            showFiltersToolStripMenuItem.Name = "showFiltersToolStripMenuItem";
            showFiltersToolStripMenuItem.Size = new Size(137, 22);
            showFiltersToolStripMenuItem.Text = "Show Filters";
            showFiltersToolStripMenuItem.Click += ShowFiltersToolStripMenuItem_Click;
            // 
            // hideFiltersToolStripMenuItem
            // 
            hideFiltersToolStripMenuItem.Name = "hideFiltersToolStripMenuItem";
            hideFiltersToolStripMenuItem.Size = new Size(137, 22);
            hideFiltersToolStripMenuItem.Text = "Hide Filters";
            hideFiltersToolStripMenuItem.Click += HideFiltersToolStripMenuItem_Click;
            // 
            // DgvLogEvents
            // 
            DgvLogEvents.AllowUserToAddRows = false;
            DgvLogEvents.AllowUserToDeleteRows = false;
            DgvLogEvents.AllowUserToResizeRows = false;
            DgvLogEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvLogEvents.Columns.AddRange(new DataGridViewColumn[] { DgcTimestamp, DgcLevel, DgcMessage });
            DgvLogEvents.Dock = DockStyle.Fill;
            DgvLogEvents.EditMode = DataGridViewEditMode.EditProgrammatically;
            DgvLogEvents.Location = new Point(0, 0);
            DgvLogEvents.Margin = new Padding(0);
            DgvLogEvents.MultiSelect = false;
            DgvLogEvents.Name = "DgvLogEvents";
            DgvLogEvents.RowHeadersVisible = false;
            DgvLogEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvLogEvents.Size = new Size(1117, 295);
            DgvLogEvents.TabIndex = 1;
            DgvLogEvents.VirtualMode = true;
            DgvLogEvents.CellValueNeeded += DgvLogEvents_CellValueNeeded;
            // 
            // DgcTimestamp
            // 
            DgcTimestamp.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgcTimestamp.HeaderText = "Timestamp";
            DgcTimestamp.Name = "DgcTimestamp";
            DgcTimestamp.ReadOnly = true;
            DgcTimestamp.Width = 145;
            // 
            // DgcLevel
            // 
            DgcLevel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgcLevel.HeaderText = "Level";
            DgcLevel.Name = "DgcLevel";
            DgcLevel.ReadOnly = true;
            DgcLevel.Width = 80;
            // 
            // DgcMessage
            // 
            DgcMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgcMessage.HeaderText = "Message";
            DgcMessage.Name = "DgcMessage";
            DgcMessage.ReadOnly = true;
            // 
            // TlpFilters
            // 
            TlpFilters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            TlpFilters.AutoSize = true;
            TlpFilters.ColumnCount = 5;
            TlpFilters.ColumnStyles.Add(new ColumnStyle());
            TlpFilters.ColumnStyles.Add(new ColumnStyle());
            TlpFilters.ColumnStyles.Add(new ColumnStyle());
            TlpFilters.ColumnStyles.Add(new ColumnStyle());
            TlpFilters.ColumnStyles.Add(new ColumnStyle());
            TlpFilters.Controls.Add(CblExceptions, 3, 2);
            TlpFilters.Controls.Add(label1, 0, 0);
            TlpFilters.Controls.Add(CmbStartDate, 0, 1);
            TlpFilters.Controls.Add(label2, 0, 3);
            TlpFilters.Controls.Add(DtpStartTime, 0, 2);
            TlpFilters.Controls.Add(CmbEndDate, 0, 4);
            TlpFilters.Controls.Add(DtpEndTime, 0, 5);
            TlpFilters.Controls.Add(label5, 1, 0);
            TlpFilters.Controls.Add(TlpProperties, 4, 0);
            TlpFilters.Controls.Add(CblLevel, 1, 2);
            TlpFilters.Controls.Add(CblMessageTemplate, 2, 2);
            TlpFilters.Controls.Add(label3, 1, 1);
            TlpFilters.Controls.Add(label4, 2, 1);
            TlpFilters.Controls.Add(TxtMessage, 2, 0);
            TlpFilters.Controls.Add(LblExceptions, 3, 1);
            TlpFilters.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            TlpFilters.Location = new Point(0, 0);
            TlpFilters.Margin = new Padding(0);
            TlpFilters.Name = "TlpFilters";
            TlpFilters.RowCount = 6;
            TlpFilters.RowStyles.Add(new RowStyle());
            TlpFilters.RowStyles.Add(new RowStyle());
            TlpFilters.RowStyles.Add(new RowStyle());
            TlpFilters.RowStyles.Add(new RowStyle());
            TlpFilters.RowStyles.Add(new RowStyle());
            TlpFilters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TlpFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TlpFilters.Size = new Size(1069, 176);
            TlpFilters.TabIndex = 0;
            // 
            // CblExceptions
            // 
            CblExceptions.CheckOnClick = true;
            CblExceptions.FormattingEnabled = true;
            CblExceptions.Location = new Point(644, 61);
            CblExceptions.Name = "CblExceptions";
            TlpFilters.SetRowSpan(CblExceptions, 4);
            CblExceptions.Size = new Size(322, 58);
            CblExceptions.TabIndex = 7;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(3, 14);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 0;
            label1.Text = "Start:";
            // 
            // CmbStartDate
            // 
            CmbStartDate.FormattingEnabled = true;
            CmbStartDate.Location = new Point(3, 32);
            CmbStartDate.Name = "CmbStartDate";
            CmbStartDate.Size = new Size(85, 23);
            CmbStartDate.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(3, 87);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 0;
            label2.Text = "End:";
            // 
            // DtpStartTime
            // 
            DtpStartTime.Format = DateTimePickerFormat.Time;
            DtpStartTime.Location = new Point(3, 61);
            DtpStartTime.Name = "DtpStartTime";
            DtpStartTime.ShowUpDown = true;
            DtpStartTime.Size = new Size(85, 23);
            DtpStartTime.TabIndex = 3;
            // 
            // CmbEndDate
            // 
            CmbEndDate.FormattingEnabled = true;
            CmbEndDate.Location = new Point(3, 105);
            CmbEndDate.Name = "CmbEndDate";
            CmbEndDate.Size = new Size(85, 23);
            CmbEndDate.TabIndex = 2;
            // 
            // DtpEndTime
            // 
            DtpEndTime.Format = DateTimePickerFormat.Time;
            DtpEndTime.Location = new Point(3, 134);
            DtpEndTime.Name = "DtpEndTime";
            DtpEndTime.ShowUpDown = true;
            DtpEndTime.Size = new Size(85, 23);
            DtpEndTime.TabIndex = 3;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(172, 7);
            label5.Name = "label5";
            label5.Size = new Size(56, 15);
            label5.TabIndex = 0;
            label5.Text = "Message:";
            // 
            // TlpProperties
            // 
            TlpProperties.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            TlpProperties.AutoSize = true;
            TlpProperties.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TlpProperties.ColumnCount = 1;
            TlpProperties.ColumnStyles.Add(new ColumnStyle());
            TlpProperties.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            TlpProperties.Location = new Point(969, 0);
            TlpProperties.Margin = new Padding(0);
            TlpProperties.MinimumSize = new Size(100, 0);
            TlpProperties.Name = "TlpProperties";
            TlpProperties.RowCount = 2;
            TlpFilters.SetRowSpan(TlpProperties, 6);
            TlpProperties.RowStyles.Add(new RowStyle());
            TlpProperties.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TlpProperties.Size = new Size(100, 176);
            TlpProperties.TabIndex = 6;
            // 
            // CblLevel
            // 
            CblLevel.CheckOnClick = true;
            CblLevel.FormattingEnabled = true;
            CblLevel.Location = new Point(94, 61);
            CblLevel.Name = "CblLevel";
            TlpFilters.SetRowSpan(CblLevel, 4);
            CblLevel.Size = new Size(134, 22);
            CblLevel.TabIndex = 4;
            // 
            // CblMessageTemplate
            // 
            CblMessageTemplate.CheckOnClick = true;
            CblMessageTemplate.FormattingEnabled = true;
            CblMessageTemplate.Location = new Point(234, 61);
            CblMessageTemplate.Name = "CblMessageTemplate";
            TlpFilters.SetRowSpan(CblMessageTemplate, 4);
            CblMessageTemplate.Size = new Size(404, 58);
            CblMessageTemplate.TabIndex = 4;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(94, 43);
            label3.Name = "label3";
            label3.Size = new Size(37, 15);
            label3.TabIndex = 0;
            label3.Text = "Level:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(234, 43);
            label4.Name = "label4";
            label4.Size = new Size(105, 15);
            label4.TabIndex = 0;
            label4.Text = "MessageTemplate:";
            // 
            // TxtMessage
            // 
            TlpFilters.SetColumnSpan(TxtMessage, 2);
            TxtMessage.Location = new Point(234, 3);
            TxtMessage.Name = "TxtMessage";
            TxtMessage.Size = new Size(404, 23);
            TxtMessage.TabIndex = 5;
            // 
            // LblExceptions
            // 
            LblExceptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LblExceptions.AutoSize = true;
            LblExceptions.Location = new Point(644, 43);
            LblExceptions.Name = "LblExceptions";
            LblExceptions.Size = new Size(66, 15);
            LblExceptions.TabIndex = 0;
            LblExceptions.Text = "Exceptions:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { LblEventCount });
            statusStrip1.Location = new Point(0, 503);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1117, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // LblEventCount
            // 
            LblEventCount.Name = "LblEventCount";
            LblEventCount.Size = new Size(137, 17);
            LblEventCount.Text = "No log events to display.";
            // 
            // SctSplitter
            // 
            SctSplitter.BackColor = Color.CornflowerBlue;
            SctSplitter.Dock = DockStyle.Fill;
            SctSplitter.FixedPanel = FixedPanel.Panel1;
            SctSplitter.Location = new Point(0, 24);
            SctSplitter.Name = "SctSplitter";
            SctSplitter.Orientation = Orientation.Horizontal;
            // 
            // SctSplitter.Panel1
            // 
            SctSplitter.Panel1.AutoScroll = true;
            SctSplitter.Panel1.BackColor = SystemColors.Control;
            SctSplitter.Panel1.Controls.Add(TlpFilters);
            SctSplitter.Panel1MinSize = 176;
            // 
            // SctSplitter.Panel2
            // 
            SctSplitter.Panel2.Controls.Add(DgvLogEvents);
            SctSplitter.Size = new Size(1117, 479);
            SctSplitter.SplitterDistance = 176;
            SctSplitter.SplitterWidth = 8;
            SctSplitter.TabIndex = 5;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(24, 20);
            toolStripMenuItem1.Text = "?";
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            // 
            // LogInspectorDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1117, 525);
            Controls.Add(SctSplitter);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "LogInspectorDlg";
            Text = "LogInspector";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DgvLogEvents).EndInit();
            TlpFilters.ResumeLayout(false);
            TlpFilters.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            SctSplitter.Panel1.ResumeLayout(false);
            SctSplitter.Panel1.PerformLayout();
            SctSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SctSplitter).EndInit();
            SctSplitter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem LoadLogFilesToolStripMenuItem;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private DataGridView DgvLogEvents;
        private TableLayoutPanel TlpFilters;
        private Label label1;
        private Label label2;
        private ComboBox CmbStartDate;
        private ComboBox CmbEndDate;
        private DateTimePicker DtpStartTime;
        private DateTimePicker DtpEndTime;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel LblEventCount;
        private DataGridViewTextBoxColumn DgcTimestamp;
        private DataGridViewTextBoxColumn DgcLevel;
        private DataGridViewTextBoxColumn DgcMessage;
        private CheckBoxList CblLevel;
        private Label label3;
        private CheckBoxList CblMessageTemplate;
        private Label label4;
        private Label label5;
        private TextBox TxtMessage;
        private TableLayoutPanel TlpProperties;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem showFiltersToolStripMenuItem;
        private ToolStripMenuItem hideFiltersToolStripMenuItem;
        private CheckBoxList CblExceptions;
        private Label LblExceptions;
        private SplitContainer SctSplitter;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}
