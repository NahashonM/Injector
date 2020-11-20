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
