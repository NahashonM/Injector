using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace injector
{
    internal struct INJECTION_DLL
    {
        public string name;
        public string path;
        public string arch;
    }

    public partial class maingui : Form
    {
        private enum APP_MODE { HandleElevation, FileInjection };

        private bool    windowDragEvt = false;                 // Custom title bar mouse onmove event tracker
        private int     mouseX, mouseY;                        // Custom title bar Mouse position trackers

        ProcessInfo     SelectedProcess;                        // Process Picked by user
        DataTable       fileDataTable;                         // Data source .. Files to be injected
        DataTable       handleDataTable;                         // Data source .. Files to be injected
        List<string>    InjectionMethods;                       // Data source .. Files to be injected
        List<string>    AvailableDrivers;                       // Data source .. Files to be injected

        string[] accepted_extensions = { "exe", "dll" };

        public maingui()
        {
            InitializeComponent();

            // Init global data
            AvailableDrivers = new List<string>();
            InjectionMethods = new List<string>();
            fileDataTable = new DataTable();
            handleDataTable = new DataTable();

            // Query available injection methods
            Tasks.Task.QueryAvailableInjectionMethods(ref InjectionMethods);
            cboInjectionMethods.DataSource = InjectionMethods;

            // Init data tables
            InitDataTables();

            // Load previous user configurations
            LoadDefaultSettings();
        }

        /// <summary>
        /// Init the Data table column headers. 
        /// </summary>
        private void InitDataTables()
        {
            // Files data table
            fileDataTable.Columns.Add("inject", typeof(bool));
            fileDataTable.Columns.Add("fileName", typeof(string));
            fileDataTable.Columns.Add("filePath", typeof(string));
            fileDataTable.Columns.Add("fileArch", typeof(string));

            // Handles data table
            handleDataTable.Columns.Add("elevate", typeof(bool));
            handleDataTable.Columns.Add("hValue", typeof(ulong));
            handleDataTable.Columns.Add("grAccess", typeof(uint));
            handleDataTable.Columns.Add("dsAccess", typeof(uint));
        }


        /// <summary>
        /// Switch Data tables based on seleceted mode
        /// </summary>
        /// <param name="mode">APP_MODE.[injection|elevation]</param>
        private void SwitchDataTable(APP_MODE mode)
        {
            if (mode == APP_MODE.FileInjection)
            {
                dtvFileSelections.DataSource = fileDataTable;
                SetFileDataTableStyle();

                // Connect event listeners
                dtvFileSelections.DragDrop += DtvFileSelections_DragDrop;
                dtvFileSelections.DragEnter += DtvFileSelections_DragEnter;
                dtvFileSelections.MouseDoubleClick += DtvFileSelections_MouseDoubleClick;

                // Allow user adding files
                dtvFileSelections.AllowUserToDeleteRows = true;
            }
            else
            {
                dtvFileSelections.DataSource = handleDataTable;
                SetHandleDataTableStyle();

                // Disconnect event listeners
                dtvFileSelections.DragDrop -= DtvFileSelections_DragDrop;
                dtvFileSelections.DragEnter -= DtvFileSelections_DragEnter;
                dtvFileSelections.MouseDoubleClick -= DtvFileSelections_MouseDoubleClick;

                // prevent user adding files
                dtvFileSelections.AllowUserToDeleteRows = false;
            }
        }
    

        /// <summary>
        /// Set Visual styles for injection files datagrid view
        /// </summary>
        private void SetFileDataTableStyle()
        {
            dtvFileSelections.Columns[0].HeaderText = "";
            dtvFileSelections.Columns[0].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[0].Width = 20;

            dtvFileSelections.Columns[1].HeaderText = "File Name";
            dtvFileSelections.Columns[1].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[1].MinimumWidth = 100;
            dtvFileSelections.Columns[1].Width = 120;
            dtvFileSelections.Columns[1].ReadOnly = true;

            dtvFileSelections.Columns[2].HeaderText = "File Path";
            dtvFileSelections.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[2].MinimumWidth = 180;
            //dtvFileSelections.Columns[2].Width = 220;
            dtvFileSelections.Columns[2].ReadOnly = true;


            dtvFileSelections.Columns[3].HeaderText = "Arch";
            dtvFileSelections.Columns[3].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[3].Width = 50;
            dtvFileSelections.Columns[3].ReadOnly = true;
        }

        /// <summary>
        /// Set Visual styles for handles table
        /// </summary>
        private void SetHandleDataTableStyle()
        {
            dtvFileSelections.Columns[0].HeaderText = "";
            dtvFileSelections.Columns[0].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[0].Width = 20;

            dtvFileSelections.Columns[1].HeaderText = "Handle value";
            dtvFileSelections.Columns[1].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[1].MinimumWidth = 100;
            dtvFileSelections.Columns[1].Width = 120;
            dtvFileSelections.Columns[1].ReadOnly = true;

            dtvFileSelections.Columns[2].HeaderText = "Current Access";
            dtvFileSelections.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[2].ReadOnly = true;


            dtvFileSelections.Columns[3].HeaderText = "Desired Access";
            dtvFileSelections.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[3].ReadOnly = false;
        }


        /// <summary>
        /// Add a file to the injection list
        /// </summary>
        /// <param name="fileList">list of files to be injected</param>
        private void AddNewFile(string[] fileList)
        {

            foreach (var file in fileList)
            {
                var extension = file.Substring(file.LastIndexOf('.') + 1);

                if (accepted_extensions.Contains<string>(extension.ToLower()))
                {
                    var rows = fileDataTable.AsEnumerable().Where(r => r.Field<string>("filePath") == file);
                    if (rows.Count() > 0)
                    {
                        MessageBox.Show("File Arleady Added");
                        continue;
                    }

                    DataRow row = fileDataTable.NewRow();
                    row["inject"] = true;
                    row["fileName"] = file.Substring(file.LastIndexOf('\\') + 1); ;
                    row["filePath"] = file;
                    row["fileArch"] = Natives.GetImageArchitecture(file);

                    fileDataTable.Rows.Add(row);
                }
            }
        }



        /// <summary>
        /// CustomTitleBar_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            windowDragEvt = true;
            mouseX = MousePosition.X;
            mouseY = MousePosition.Y;
        }

        /// <summary>
        /// CustomTitleBar_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (windowDragEvt)
            {
                int mx = MousePosition.X, my = MousePosition.Y;

                SetBounds(Bounds.Left - mouseX + mx, Bounds.Top - mouseY + my, Size.Width, Size.Height);
                mouseX = mx;
                mouseY = my;
            }
        }

        /// <summary>
        /// CustomTitleBar_MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            windowDragEvt = false;
        }

        /// <summary>
        /// BtnClose_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// BtnMinimize_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Process Picker Callback
        /// </summary>
        /// <param name="selectedProcess"></param>
        public void ProcessSelected_Callback(ProcessInfo selectedProcess)
        {
            SelectedProcess = selectedProcess;

            imgProcessIcon.Image = SelectedProcess.Icon;
            lblPid.Text = SelectedProcess.Pid.ToString() + " : 0x" + SelectedProcess.Pid.ToString("X");
            lblName.Text = SelectedProcess.Name;
            lblArch.Text = SelectedProcess.Arch;
        }

        /// <summary>
        /// Drag Enter Event Listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtvFileSelections_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))    // Check if file drop
                e.Effect = DragDropEffects.Move;                // modify the drag drop effects to Move
            else
                e.Effect = DragDropEffects.None;                // no need for any drag drop effect
        }

        /// <summary>
        /// Drag Drop Event Listener [modified DragEnter]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtvFileSelections_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))        // still check if the associated data 
                AddNewFile((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        /// <summary>
        /// Data grid View Double Click event listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtvFileSelections_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.ShowDialog();

            string[] fileNames = fileDialog.FileNames;

            AddNewFile(fileNames);
        }

        /// <summary>
        /// Dispatch injection parameters to the FileInjector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInject_Click(object sender, EventArgs e)
        {
            int targetPid = 0;
            try  {
                targetPid = Int32.Parse(lblPid.Text.Substring(0, lblPid.Text.IndexOf(' ')));
            } catch (Exception)  {
                MessageBox.Show("Please Select a process to inject into");
                return;
            }

            if (rdInjectionMode.Checked)
                DispatchInjectionTask(targetPid);
            else
                DispatchElevationTask(targetPid);
            
        }




        /// <summary>
        /// Calls for dispatch of injection task
        /// </summary>
        /// <param name="pid"></param>
        private void DispatchInjectionTask(int pid)
        {
            List<string> files = new List<string> { };
            foreach (DataRow row in fileDataTable.Rows)
            {
                if ((bool)row["inject"] == true)
                    files.Add(row["filePath"].ToString());
            }


            if (files.Count() < 1)
            {
                MessageBox.Show("Please Add files to inject");
                return;
            }

            Tasks.InjectionModel injectionModel = new Tasks.InjectionModel
            {
                TargetPid = pid,
                InjectionMethod = cboInjectionMethods.SelectedValue?.ToString(),
                HijackHandle = chkHijackHandle.Checked,
                ElevateHandle = chkElevateHandle.Checked,
                ObtainHandleViaDriver = chkDriverObtainHandle.Checked,
                UnloadOnInject = chkUnloadAfterInject.Checked,
                FilesList = files
            };

            Tasks.Task.StartTask(injectionModel);

            return;
        }


        /// <summary>
        /// calls for a dispatch of handle elevation task
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool DispatchElevationTask(int pid)
        {
            Tasks.ElevationModel elevationModel = new Tasks.ElevationModel
            {
                ProcessID = pid,
                Handles = new List<uint> { }
            };

            return Tasks.Task.StartTask(elevationModel);
        }


        /// <summary>
        /// Window Load event listener.. Sets previous config as base
        /// </summary>
        private void LoadDefaultSettings()
        {
            chkElevateHandle.Checked = Properties.Settings.Default.elevateHandle;
            chkUnloadAfterInject.Checked = Properties.Settings.Default.unloadDriverOnInject;
            chkHijackHandle.Checked = Properties.Settings.Default.hijackHandle;
            chkDriverObtainHandle.Checked = Properties.Settings.Default.driverObtainHandles;

            // TODO----
            // Implement previous method history revert
            //cboInjectionMethods. = Properties.Settings.Default.previousMethod;


            string previousFiles = Properties.Settings.Default.previousFiles;

            foreach (string file in previousFiles.Split('\n'))
            {
                string[] entry = file.Split(';');
                if (entry.Count() > 1)
                {
                    DataRow row = fileDataTable.NewRow();
                    row["inject"] = (entry[1] == "True") ? true : false;
                    row["fileName"] = entry[0].Substring(entry[0].LastIndexOf('\\') + 1); ;
                    row["filePath"] = entry[0];
                    row["fileArch"] = Natives.GetImageArchitecture(entry[0]);

                    fileDataTable.Rows.Add(row);
                }
            }


            if (Properties.Settings.Default.isInjectionMode)
            {
                SwitchDataTable(APP_MODE.FileInjection);

                rdElevationMode.Checked = false;
                rdInjectionMode.Checked = true;

            }
            else
            {
                SwitchDataTable(APP_MODE.HandleElevation);

                rdElevationMode.Checked = true;
                rdInjectionMode.Checked = false;

                grpInjectionMethod.Enabled = false;
            }
        }


        private void Maingui_Load(object sender, EventArgs e)
        {
           
        }


        /// <summary>
        /// OnForm closing evt.. Update user settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maingui_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.isInjectionMode = rdInjectionMode.Checked;
            Properties.Settings.Default.elevateHandle = chkElevateHandle.Checked;
            Properties.Settings.Default.unloadDriverOnInject = chkUnloadAfterInject.Checked;
            Properties.Settings.Default.hijackHandle = chkHijackHandle.Checked;

            Properties.Settings.Default.previousMethod = cboInjectionMethods.SelectedValue?.ToString();
            Properties.Settings.Default.driverObtainHandles = chkDriverObtainHandle.Checked;

            try {
                Int32.Parse(lblPid.Text.Substring(0, lblPid.Text.IndexOf(' ')));
                Properties.Settings.Default.previousProcessName = lblName.Text;
            }
            catch (Exception){
                Properties.Settings.Default.previousProcessName = null;
            }

            string files = null;
            foreach (DataRow row in fileDataTable.Rows)
            {
                files += row["filePath"].ToString() + ";"+ row["inject"].ToString() + "\n";
            }

            Properties.Settings.Default.previousFiles = files;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Mode change event listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdInjectionMode_CheckedChanged(object sender, EventArgs e)
        {
            if (grpInjectionMethod.Enabled = rdInjectionMode.Checked)
            {
                SwitchDataTable(APP_MODE.FileInjection);

                btnInject.Text = "Inject File";
                btnAddFile.Visible = true;
                btnDeleteFile.Visible = true;
                btnRefresh.Visible = false;
            }
            else
            {
                SwitchDataTable(APP_MODE.HandleElevation);

                btnInject.Text = "Elevate Handle";
                btnAddFile.Visible = false;
                btnDeleteFile.Visible = false;
                btnRefresh.Visible = true;
            }
        }

        /// <summary>
        /// Pick Process Button Event Listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPickProcess_Click(object sender, EventArgs e)
        {
            var ProcessPicker = new Forms.ProcessPickerDialog();
            ProcessPicker.selected_Callback = ProcessSelected_Callback;
            ProcessPicker.ShowDialog();
        }

    }
}
