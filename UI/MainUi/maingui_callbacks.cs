
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace injector
{

    public partial class maingui
    {

        string[] accepted_extensions = { "exe", "dll" };


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
        /// Add a handle to the handle list
        /// </summary>
        /// <param name="handleValue">handle values [handle value ; granted access ; name]</param>
        private void NewHandleFound_Callback(UIntPtr handle, UInt32 accessMask, string type, string name)
        {
            DataRow row = handleDataTable.NewRow();

			row["elevate"] = false;                         // bool
			row["hValue"] = ((UInt32)handle).ToString("X");	// string
			row["hType"] = type;							// string
			row["hName"] = name;							// string
            row["grAccess"] = accessMask.ToString("X");		// uint
            row["dsAccess"] = accessMask.ToString("X");     // uint

			this.BeginInvoke(new Action(() => handleDataTable.Rows.Add(row)));
			
            
        }


        /// <summary>
        /// Called once a task is completed
        /// </summary>
        /// <param name="task">Type of task</param>
        /// <param name="errorCode">Exit code</param>
        private void TaskCompleteCallback()
        {

                this.BeginInvoke(new Action(() => dtvFileSelections.Refresh()));
                this.BeginInvoke(new Action(() => btnRefreshHandles.Enabled = true));

                this.BeginInvoke(new Action(() => dtvFileSelections.ScrollBars = ScrollBars.Vertical));
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        void MethodIdentifiedCallback(String methodName)
        {
            InjectionMethods.Add(methodName);
        }


        /// <summary>
        /// Process Picker Callback
        /// </summary>
        /// <param name="selectedProcess"></param>
        public void ProcessSelected_Callback(ProcessInfo selectedProcess)
        {
            uint previousProcessPid = 0;            

            previousProcessPid = SelectedProcess.Pid;
            SelectedProcess = selectedProcess;

            imgProcessIcon.Image = SelectedProcess.Icon;
            lblPid.Text = SelectedProcess.Pid.ToString() + " : 0x" + SelectedProcess.Pid.ToString("X");
            lblName.Text = SelectedProcess.Name;
            lblArch.Text = SelectedProcess.Arch;

            // If different process was selected
            if (previousProcessPid != selectedProcess.Pid)
            {

                if (rdElevationMode.Checked) {
                    BtnRefreshHandles_Click(null, null);        // Refresh handles
                } else {
                    // TODO -----
                    // Clear files whose architecture is different from that 
                    // of new process
                }
            }

        }






	}


}
