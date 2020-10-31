namespace injector.Forms
{
    partial class ProcessPickerDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.procListView = new System.Windows.Forms.DataGridView();
            this.customTitleBar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.pnlRefineSearch = new System.Windows.Forms.Panel();
            this.chkFilterSystemProcesses = new System.Windows.Forms.CheckBox();
            this.btnSelectProcess = new System.Windows.Forms.Button();
            this.lblTxtFilter = new System.Windows.Forms.Label();
            this.txtFilterString = new System.Windows.Forms.TextBox();
            this.btnRefreshProcesses = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.procListView)).BeginInit();
            this.customTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.pnlRefineSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // procListView
            // 
            this.procListView.AllowUserToAddRows = false;
            this.procListView.AllowUserToDeleteRows = false;
            this.procListView.AllowUserToOrderColumns = true;
            this.procListView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(89)))), ((int)(((byte)(89)))));
            this.procListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.procListView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(89)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(89)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.procListView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.procListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.procListView.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.procListView.DefaultCellStyle = dataGridViewCellStyle2;
            this.procListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.procListView.EnableHeadersVisualStyles = false;
            this.procListView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.procListView.Location = new System.Drawing.Point(0, 0);
            this.procListView.MultiSelect = false;
            this.procListView.Name = "procListView";
            this.procListView.ReadOnly = true;
            this.procListView.RowHeadersVisible = false;
            this.procListView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.procListView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(89)))), ((int)(((byte)(89)))));
            this.procListView.RowTemplate.ReadOnly = true;
            this.procListView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.procListView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.procListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.procListView.ShowEditingIcon = false;
            this.procListView.Size = new System.Drawing.Size(499, 356);
            this.procListView.TabIndex = 0;
            this.procListView.DoubleClick += new System.EventHandler(this.BtnSelectProcess_Click);
            this.procListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProcListView_KeyDown);
            // 
            // customTitleBar
            // 
            this.customTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.customTitleBar.Controls.Add(this.btnClose);
            this.customTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.customTitleBar.Location = new System.Drawing.Point(0, 0);
            this.customTitleBar.Name = "customTitleBar";
            this.customTitleBar.Size = new System.Drawing.Size(499, 26);
            this.customTitleBar.TabIndex = 1;
            this.customTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseDown);
            this.customTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseMove);
            this.customTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.ErrorImage = global::injector.Properties.Resources.close_icon1;
            this.btnClose.Image = global::injector.Properties.Resources.close_icon1;
            this.btnClose.InitialImage = global::injector.Properties.Resources.close_icon1;
            this.btnClose.Location = new System.Drawing.Point(472, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(27, 26);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // pnlRefineSearch
            // 
            this.pnlRefineSearch.Controls.Add(this.chkFilterSystemProcesses);
            this.pnlRefineSearch.Controls.Add(this.btnSelectProcess);
            this.pnlRefineSearch.Controls.Add(this.lblTxtFilter);
            this.pnlRefineSearch.Controls.Add(this.txtFilterString);
            this.pnlRefineSearch.Controls.Add(this.btnRefreshProcesses);
            this.pnlRefineSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRefineSearch.Location = new System.Drawing.Point(0, 26);
            this.pnlRefineSearch.Name = "pnlRefineSearch";
            this.pnlRefineSearch.Size = new System.Drawing.Size(499, 118);
            this.pnlRefineSearch.TabIndex = 2;
            // 
            // chkFilterSystemProcesses
            // 
            this.chkFilterSystemProcesses.AutoSize = true;
            this.chkFilterSystemProcesses.Location = new System.Drawing.Point(15, 57);
            this.chkFilterSystemProcesses.Name = "chkFilterSystemProcesses";
            this.chkFilterSystemProcesses.Size = new System.Drawing.Size(137, 17);
            this.chkFilterSystemProcesses.TabIndex = 4;
            this.chkFilterSystemProcesses.Text = "Filter System Processes";
            this.chkFilterSystemProcesses.UseVisualStyleBackColor = true;
            this.chkFilterSystemProcesses.CheckStateChanged += new System.EventHandler(this.ChkFilterSystemProcesses_CheckStateChanged);
            // 
            // btnSelectProcess
            // 
            this.btnSelectProcess.Location = new System.Drawing.Point(183, 89);
            this.btnSelectProcess.Name = "btnSelectProcess";
            this.btnSelectProcess.Size = new System.Drawing.Size(104, 23);
            this.btnSelectProcess.TabIndex = 3;
            this.btnSelectProcess.Text = "Select Process";
            this.btnSelectProcess.UseVisualStyleBackColor = true;
            this.btnSelectProcess.Click += new System.EventHandler(this.BtnSelectProcess_Click);
            // 
            // lblTxtFilter
            // 
            this.lblTxtFilter.AutoSize = true;
            this.lblTxtFilter.Location = new System.Drawing.Point(12, 24);
            this.lblTxtFilter.Name = "lblTxtFilter";
            this.lblTxtFilter.Size = new System.Drawing.Size(81, 13);
            this.lblTxtFilter.TabIndex = 2;
            this.lblTxtFilter.Text = "Filter Processes";
            // 
            // txtFilterString
            // 
            this.txtFilterString.Location = new System.Drawing.Point(99, 17);
            this.txtFilterString.Name = "txtFilterString";
            this.txtFilterString.Size = new System.Drawing.Size(188, 20);
            this.txtFilterString.TabIndex = 0;
            this.txtFilterString.TextChanged += new System.EventHandler(this.TxtFilterString_TextChanged);
            // 
            // btnRefreshProcesses
            // 
            this.btnRefreshProcesses.Location = new System.Drawing.Point(15, 89);
            this.btnRefreshProcesses.Name = "btnRefreshProcesses";
            this.btnRefreshProcesses.Size = new System.Drawing.Size(118, 23);
            this.btnRefreshProcesses.TabIndex = 1;
            this.btnRefreshProcesses.Text = "Refresh Process List";
            this.btnRefreshProcesses.UseVisualStyleBackColor = true;
            this.btnRefreshProcesses.Click += new System.EventHandler(this.BtnRefreshProcesses_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 10);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.procListView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 144);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(499, 356);
            this.panel2.TabIndex = 4;
            // 
            // ProcessPickerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.ClientSize = new System.Drawing.Size(499, 510);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlRefineSearch);
            this.Controls.Add(this.customTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProcessPickerDialog";
            this.Text = "Process Picker";
            this.Load += new System.EventHandler(this.ProcessPickerDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.procListView)).EndInit();
            this.customTitleBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.pnlRefineSearch.ResumeLayout(false);
            this.pnlRefineSearch.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView procListView;
        private System.Windows.Forms.Panel customTitleBar;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Panel pnlRefineSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtFilterString;
        private System.Windows.Forms.Label lblTxtFilter;
        private System.Windows.Forms.Button btnRefreshProcesses;
        private System.Windows.Forms.Button btnSelectProcess;
        private System.Windows.Forms.CheckBox chkFilterSystemProcesses;
    }
}