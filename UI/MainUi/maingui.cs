using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;

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

        
        ProcessInfo     SelectedProcess;                        // Process Picked by user
        DataTable       fileDataTable;                         // Data source .. Files to be injected
        DataTable       handleDataTable;                         // Data source .. Files to be injected
        List<string>    InjectionMethods;                       // Data source .. Files to be injected
        List<string>    AvailableDrivers;                       // Data source .. Files to be injected

        

        public maingui()
        {
            InitializeComponent();

            // Init global data
            SelectedProcess = new ProcessInfo();
            AvailableDrivers = new List<string>();
            InjectionMethods = new List<string>();
            fileDataTable = new DataTable();
            handleDataTable = new DataTable();

            // Query available injection methods
            Tasks.Task.QueryInjectionMethods(MethodIdentifiedCallback, TaskCompleteCallback);
            cboInjectionMethods.DataSource = InjectionMethods;

            // Init data tables
            InitDataTables();

            // Load previous user configurations
            LoadDefaultSettings();
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
                if ((bool)row["inject"] == true && (string)row["fileArch"] == lblArch.Text)
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
                TargetArchitecture = lblArch.Text,
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

            List<Tasks.NewHandleValue> handleList = new List<Tasks.NewHandleValue> { };

            foreach (DataRow row in handleDataTable.Rows)
            {
                if ((bool)row["elevate"] == true) {
                    IntPtr handleValue = IntPtr.Zero;
                    int accessMask = -1;
                    uint previousAccessMask = 0;

                    try {
                        handleValue = (IntPtr)((IntPtr.Size == 4) ?
                                              UInt32.Parse((string)row["hValue"], NumberStyles.HexNumber) :
                                              UInt64.Parse((string)row["hValue"], NumberStyles.HexNumber));

                        accessMask = Int32.Parse((string)row["dsAccess"], NumberStyles.HexNumber);
                        previousAccessMask = UInt32.Parse((string)row["grAccess"], NumberStyles.HexNumber);
                    } catch (FormatException e) {
                        continue;
                    }


                    handleList.Add(new Tasks.NewHandleValue()
                    {
                        hValue = handleValue,
                        AcessMask = (uint)((accessMask == previousAccessMask || accessMask >= 0) ? 0x1fffff : accessMask)
                    });
                }
            }


            if (handleList.Count() < 1)
            {
                MessageBox.Show("Please Select handle(s) to elevate");
                return false;
            }

            Tasks.ElevationModel elevationModel = new Tasks.ElevationModel
            {
                ProcessID   = pid,
                Handles     = handleList
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


            if (rdInjectionMode.Checked = Properties.Settings.Default.isInjectionMode)
                rdElevationMode.Checked = false;
            else
                rdElevationMode.Checked = true;

            RdInjectionMode_CheckedChanged(null, null);
        }



    }
}
