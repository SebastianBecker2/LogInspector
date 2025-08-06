namespace LogInspector
{
    partial class DetailsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            BtnClose = new Button();
            BtnCopyToClipboard = new Button();
            DgvDetails = new DataGridView();
            DgcTitle = new DataGridViewTextBoxColumn();
            DgcValue = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DgvDetails).BeginInit();
            SuspendLayout();
            // 
            // BtnClose
            // 
            BtnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnClose.DialogResult = DialogResult.OK;
            BtnClose.Location = new Point(429, 396);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(75, 23);
            BtnClose.TabIndex = 0;
            BtnClose.Text = "Close";
            BtnClose.UseVisualStyleBackColor = true;
            // 
            // BtnCopyToClipboard
            // 
            BtnCopyToClipboard.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCopyToClipboard.Location = new Point(299, 396);
            BtnCopyToClipboard.Name = "BtnCopyToClipboard";
            BtnCopyToClipboard.Size = new Size(124, 23);
            BtnCopyToClipboard.TabIndex = 0;
            BtnCopyToClipboard.Text = "Copy To Clipboard";
            BtnCopyToClipboard.UseVisualStyleBackColor = true;
            BtnCopyToClipboard.Click += BtnCopyToClipboard_Click;
            // 
            // DgvDetails
            // 
            DgvDetails.AllowUserToAddRows = false;
            DgvDetails.AllowUserToDeleteRows = false;
            DgvDetails.AllowUserToResizeColumns = false;
            DgvDetails.AllowUserToResizeRows = false;
            DgvDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DgvDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvDetails.ColumnHeadersVisible = false;
            DgvDetails.Columns.AddRange(new DataGridViewColumn[] { DgcTitle, DgcValue });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DgvDetails.DefaultCellStyle = dataGridViewCellStyle1;
            DgvDetails.Location = new Point(12, 12);
            DgvDetails.Name = "DgvDetails";
            DgvDetails.RowHeadersVisible = false;
            DgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvDetails.Size = new Size(492, 378);
            DgvDetails.TabIndex = 1;
            // 
            // DgcTitle
            // 
            DgcTitle.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcTitle.HeaderText = "Title";
            DgcTitle.Name = "DgcTitle";
            DgcTitle.ReadOnly = true;
            DgcTitle.Width = 5;
            // 
            // DgcValue
            // 
            DgcValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgcValue.HeaderText = "Value";
            DgcValue.Name = "DgcValue";
            DgcValue.ReadOnly = true;
            // 
            // DetailsDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnClose;
            ClientSize = new Size(516, 431);
            Controls.Add(DgvDetails);
            Controls.Add(BtnCopyToClipboard);
            Controls.Add(BtnClose);
            Name = "DetailsDlg";
            Text = "DetailsDlg";
            ((System.ComponentModel.ISupportInitialize)DgvDetails).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnClose;
        private Button BtnCopyToClipboard;
        private DataGridView DgvDetails;
        private DataGridViewTextBoxColumn DgcTitle;
        private DataGridViewTextBoxColumn DgcValue;
    }
}