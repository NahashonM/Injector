namespace injector
{
    partial class maingui
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CustomTitleBar = new System.Windows.Forms.Panel();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rdElevationMode = new System.Windows.Forms.RadioButton();
            this.rdInjectionMode = new System.Windows.Forms.RadioButton();
            this.btnMinimize = new System.Windows.Forms.PictureBox();
            this.InjectorBanner = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInject = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDriverObtainHandle = new System.Windows.Forms.CheckBox();
            this.chkUnloadAfterInject = new System.Windows.Forms.CheckBox();
            this.grpInjectionMethod = new System.Windows.Forms.GroupBox();
            this.chkElevateHandle = new System.Windows.Forms.CheckBox();
            this.cboInjectionMethods = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.chkHijackHandle = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Process = new System.Windows.Forms.GroupBox();
            this.lblArch = new System.Windows.Forms.Label();
            this.btnPickProcess = new System.Windows.Forms.Button();
            this.imgProcessIcon = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPid = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtvFileSelections = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRefreshHandles = new System.Windows.Forms.Button();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.CustomTitleBar.SuspendLayout();
            this.grpMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectorBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpInjectionMethod.SuspendLayout();
            this.panel4.SuspendLayout();
            this.Process.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgProcessIcon)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvFileSelections)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomTitleBar
            // 
            this.CustomTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.CustomTitleBar.Controls.Add(this.grpMode);
            this.CustomTitleBar.Controls.Add(this.btnMinimize);
            this.CustomTitleBar.Controls.Add(this.InjectorBanner);
            this.CustomTitleBar.Controls.Add(this.btnClose);
            this.CustomTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.CustomTitleBar.Location = new System.Drawing.Point(0, 0);
            this.CustomTitleBar.Name = "CustomTitleBar";
            this.CustomTitleBar.Size = new System.Drawing.Size(693, 41);
            this.CustomTitleBar.TabIndex = 1;
            this.CustomTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseDown);
            this.CustomTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseMove);
            this.CustomTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomTitleBar_MouseUp);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rdElevationMode);
            this.grpMode.Controls.Add(this.rdInjectionMode);
            this.grpMode.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.grpMode.Location = new System.Drawing.Point(234, 0);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(202, 38);
            this.grpMode.TabIndex = 4;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Mode";
            // 
            // rdElevationMode
            // 
            this.rdElevationMode.AutoSize = true;
            this.rdElevationMode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rdElevationMode.Location = new System.Drawing.Point(90, 16);
            this.rdElevationMode.Name = "rdElevationMode";
            this.rdElevationMode.Size = new System.Drawing.Size(106, 17);
            this.rdElevationMode.TabIndex = 1;
            this.rdElevationMode.TabStop = true;
            this.rdElevationMode.Text = "Handle Elevation";
            this.rdElevationMode.UseVisualStyleBackColor = true;
            // 
            // rdInjectionMode
            // 
            this.rdInjectionMode.AutoSize = true;
            this.rdInjectionMode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rdInjectionMode.Location = new System.Drawing.Point(6, 16);
            this.rdInjectionMode.Name = "rdInjectionMode";
            this.rdInjectionMode.Size = new System.Drawing.Size(80, 17);
            this.rdInjectionMode.TabIndex = 0;
            this.rdInjectionMode.TabStop = true;
            this.rdInjectionMode.Text = "Dll Injection";
            this.rdInjectionMode.UseVisualStyleBackColor = true;
            this.rdInjectionMode.CheckedChanged += new System.EventHandler(this.RdInjectionMode_CheckedChanged);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.ErrorImage = global::injector.Properties.Resources.minimize_icon;
            this.btnMinimize.Image = global::injector.Properties.Resources.minimize_icon;
            this.btnMinimize.InitialImage = global::injector.Properties.Resources.minimize_icon;
            this.btnMinimize.Location = new System.Drawing.Point(633, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 41);
            this.btnMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMinimize.TabIndex = 3;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);
            // 
            // InjectorBanner
            // 
            this.InjectorBanner.Enabled = false;
            this.InjectorBanner.ErrorImage = global::injector.Properties.Resources.injector_banner;
            this.InjectorBanner.Image = global::injector.Properties.Resources.injector_banner;
            this.InjectorBanner.InitialImage = global::injector.Properties.Resources.injector_banner;
            this.InjectorBanner.Location = new System.Drawing.Point(12, 3);
            this.InjectorBanner.Name = "InjectorBanner";
            this.InjectorBanner.Size = new System.Drawing.Size(113, 35);
            this.InjectorBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.InjectorBanner.TabIndex = 2;
            this.InjectorBanner.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.ErrorImage = global::injector.Properties.Resources.close_icon1;
            this.btnClose.Image = global::injector.Properties.Resources.close_icon1;
            this.btnClose.InitialImage = global::injector.Properties.Resources.close_icon1;
            this.btnClose.Location = new System.Drawing.Point(663, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 41);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnInject);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.grpInjectionMethod);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 275);
            this.panel1.TabIndex = 2;
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(15, 238);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(255, 31);
            this.btnInject.TabIndex = 2;
            this.btnInject.Text = "Inject";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.BtnInject_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDriverObtainHandle);
            this.groupBox1.Controls.Add(this.chkUnloadAfterInject);
            this.groupBox1.Location = new System.Drawing.Point(3, 186);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 36);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver";
            // 
            // chkDriverObtainHandle
            // 
            this.chkDriverObtainHandle.AutoSize = true;
            this.chkDriverObtainHandle.Location = new System.Drawing.Point(12, 16);
            this.chkDriverObtainHandle.Name = "chkDriverObtainHandle";
            this.chkDriverObtainHandle.Size = new System.Drawing.Size(85, 17);
            this.chkDriverObtainHandle.TabIndex = 8;
            this.chkDriverObtainHandle.Text = "Get Handles";
            this.chkDriverObtainHandle.UseVisualStyleBackColor = true;
            // 
            // chkUnloadAfterInject
            // 
            this.chkUnloadAfterInject.AutoSize = true;
            this.chkUnloadAfterInject.Location = new System.Drawing.Point(133, 16);
            this.chkUnloadAfterInject.Name = "chkUnloadAfterInject";
            this.chkUnloadAfterInject.Size = new System.Drawing.Size(127, 17);
            this.chkUnloadAfterInject.TabIndex = 7;
            this.chkUnloadAfterInject.Text = "Unload after Injection";
            this.chkUnloadAfterInject.UseVisualStyleBackColor = true;
            // 
            // grpInjectionMethod
            // 
            this.grpInjectionMethod.Controls.Add(this.chkElevateHandle);
            this.grpInjectionMethod.Controls.Add(this.cboInjectionMethods);
            this.grpInjectionMethod.Controls.Add(this.lblMethod);
            this.grpInjectionMethod.Controls.Add(this.chkHijackHandle);
            this.grpInjectionMethod.Location = new System.Drawing.Point(3, 116);
            this.grpInjectionMethod.Name = "grpInjectionMethod";
            this.grpInjectionMethod.Size = new System.Drawing.Size(279, 64);
            this.grpInjectionMethod.TabIndex = 7;
            this.grpInjectionMethod.TabStop = false;
            this.grpInjectionMethod.Text = "Injection Method";
            // 
            // chkElevateHandle
            // 
            this.chkElevateHandle.AutoSize = true;
            this.chkElevateHandle.Location = new System.Drawing.Point(168, 41);
            this.chkElevateHandle.Name = "chkElevateHandle";
            this.chkElevateHandle.Size = new System.Drawing.Size(99, 17);
            this.chkElevateHandle.TabIndex = 6;
            this.chkElevateHandle.Text = "Elevate Handle";
            this.chkElevateHandle.UseVisualStyleBackColor = true;
            // 
            // cboInjectionMethods
            // 
            this.cboInjectionMethods.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cboInjectionMethods.FormattingEnabled = true;
            this.cboInjectionMethods.Location = new System.Drawing.Point(60, 15);
            this.cboInjectionMethods.Name = "cboInjectionMethods";
            this.cboInjectionMethods.Size = new System.Drawing.Size(207, 21);
            this.cboInjectionMethods.TabIndex = 2;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(9, 16);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(43, 13);
            this.lblMethod.TabIndex = 4;
            this.lblMethod.Text = "Method";
            // 
            // chkHijackHandle
            // 
            this.chkHijackHandle.AutoSize = true;
            this.chkHijackHandle.Location = new System.Drawing.Point(12, 41);
            this.chkHijackHandle.Name = "chkHijackHandle";
            this.chkHijackHandle.Size = new System.Drawing.Size(93, 17);
            this.chkHijackHandle.TabIndex = 0;
            this.chkHijackHandle.Text = "Hijack Handle";
            this.chkHijackHandle.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Process);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(285, 111);
            this.panel4.TabIndex = 0;
            // 
            // Process
            // 
            this.Process.Controls.Add(this.lblArch);
            this.Process.Controls.Add(this.btnPickProcess);
            this.Process.Controls.Add(this.imgProcessIcon);
            this.Process.Controls.Add(this.lblName);
            this.Process.Controls.Add(this.lblPid);
            this.Process.Location = new System.Drawing.Point(3, 0);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(279, 110);
            this.Process.TabIndex = 0;
            this.Process.TabStop = false;
            this.Process.Text = "Process";
            // 
            // lblArch
            // 
            this.lblArch.AutoSize = true;
            this.lblArch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArch.Location = new System.Drawing.Point(78, 59);
            this.lblArch.Name = "lblArch";
            this.lblArch.Size = new System.Drawing.Size(31, 15);
            this.lblArch.TabIndex = 6;
            this.lblArch.Text = "Arch";
            // 
            // btnPickProcess
            // 
            this.btnPickProcess.Location = new System.Drawing.Point(12, 83);
            this.btnPickProcess.Name = "btnPickProcess";
            this.btnPickProcess.Size = new System.Drawing.Size(255, 23);
            this.btnPickProcess.TabIndex = 5;
            this.btnPickProcess.Text = "Process Picker";
            this.btnPickProcess.UseVisualStyleBackColor = true;
            this.btnPickProcess.Click += new System.EventHandler(this.BtnPickProcess_Click);
            // 
            // imgProcessIcon
            // 
            this.imgProcessIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imgProcessIcon.Location = new System.Drawing.Point(12, 17);
            this.imgProcessIcon.Name = "imgProcessIcon";
            this.imgProcessIcon.Size = new System.Drawing.Size(60, 60);
            this.imgProcessIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgProcessIcon.TabIndex = 4;
            this.imgProcessIcon.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(78, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // lblPid
            // 
            this.lblPid.AutoSize = true;
            this.lblPid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPid.Location = new System.Drawing.Point(78, 29);
            this.lblPid.Name = "lblPid";
            this.lblPid.Size = new System.Drawing.Size(22, 13);
            this.lblPid.TabIndex = 0;
            this.lblPid.Text = "Pid";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtvFileSelections);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(285, 41);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 233);
            this.panel2.TabIndex = 3;
            // 
            // dtvFileSelections
            // 
            this.dtvFileSelections.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dtvFileSelections.AllowDrop = true;
            this.dtvFileSelections.AllowUserToAddRows = false;
            this.dtvFileSelections.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Lime;
            this.dtvFileSelections.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtvFileSelections.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dtvFileSelections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtvFileSelections.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dtvFileSelections.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dtvFileSelections.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.NullValue = "n";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtvFileSelections.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtvFileSelections.ColumnHeadersHeight = 20;
            this.dtvFileSelections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtvFileSelections.DefaultCellStyle = dataGridViewCellStyle3;
            this.dtvFileSelections.EnableHeadersVisualStyles = false;
            this.dtvFileSelections.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.dtvFileSelections.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dtvFileSelections.Location = new System.Drawing.Point(0, 6);
            this.dtvFileSelections.Name = "dtvFileSelections";
            this.dtvFileSelections.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtvFileSelections.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dtvFileSelections.RowHeadersVisible = false;
            this.dtvFileSelections.RowHeadersWidth = 4;
            this.dtvFileSelections.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Lime;
            this.dtvFileSelections.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtvFileSelections.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dtvFileSelections.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dtvFileSelections.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dtvFileSelections.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Lime;
            this.dtvFileSelections.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dtvFileSelections.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtvFileSelections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtvFileSelections.ShowCellToolTips = false;
            this.dtvFileSelections.Size = new System.Drawing.Size(405, 227);
            this.dtvFileSelections.TabIndex = 0;
            this.dtvFileSelections.DragDrop += new System.Windows.Forms.DragEventHandler(this.DtvFileSelections_DragDrop);
            this.dtvFileSelections.DragEnter += new System.Windows.Forms.DragEventHandler(this.DtvFileSelections_DragEnter);
            this.dtvFileSelections.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DtvFileSelections_MouseDoubleClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRefreshHandles);
            this.panel3.Controls.Add(this.btnDeleteFile);
            this.panel3.Controls.Add(this.btnAddFile);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(285, 279);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(408, 37);
            this.panel3.TabIndex = 4;
            // 
            // btnRefreshHandles
            // 
            this.btnRefreshHandles.Location = new System.Drawing.Point(160, 8);
            this.btnRefreshHandles.Name = "btnRefreshHandles";
            this.btnRefreshHandles.Size = new System.Drawing.Size(104, 23);
            this.btnRefreshHandles.TabIndex = 2;
            this.btnRefreshHandles.Text = "Refresh Handles";
            this.btnRefreshHandles.UseVisualStyleBackColor = true;
            this.btnRefreshHandles.Click += new System.EventHandler(this.BtnRefreshHandles_Click);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.Location = new System.Drawing.Point(270, 8);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteFile.TabIndex = 1;
            this.btnDeleteFile.Text = "Delete File";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(79, 8);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(75, 23);
            this.btnAddFile.TabIndex = 0;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            // 
            // maingui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(693, 316);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CustomTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "maingui";
            this.Text = "Inj3ct0r";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Maingui_FormClosing);
            this.Load += new System.EventHandler(this.Maingui_Load);
            this.CustomTitleBar.ResumeLayout(false);
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectorBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpInjectionMethod.ResumeLayout(false);
            this.grpInjectionMethod.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.Process.ResumeLayout(false);
            this.Process.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgProcessIcon)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtvFileSelections)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel CustomTitleBar;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox InjectorBanner;
        private System.Windows.Forms.PictureBox btnMinimize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dtvFileSelections;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox Process;
        private System.Windows.Forms.PictureBox imgProcessIcon;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnPickProcess;
        private System.Windows.Forms.Label lblArch;
        private System.Windows.Forms.Label lblPid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboInjectionMethods;
        private System.Windows.Forms.GroupBox grpInjectionMethod;
        private System.Windows.Forms.CheckBox chkElevateHandle;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.CheckBox chkHijackHandle;
        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.CheckBox chkUnloadAfterInject;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rdElevationMode;
        private System.Windows.Forms.RadioButton rdInjectionMode;
        private System.Windows.Forms.Button btnRefreshHandles;
        private System.Windows.Forms.CheckBox chkDriverObtainHandle;
    }
}

