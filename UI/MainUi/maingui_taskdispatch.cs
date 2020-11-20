
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;


namespace injector
{

    public partial class maingui
    {

        #region ELEVATION TASK DISPATCH


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
                if ((bool)row["elevate"] == true)
                {
                    IntPtr handleValue = IntPtr.Zero;
                    int accessMask = -1;
                    uint previousAccessMask = 0;

                    try
                    {
                        handleValue = (IntPtr)((IntPtr.Size == 4) ?
                                              UInt32.Parse((string)row["hValue"], NumberStyles.HexNumber) :
                                              UInt64.Parse((string)row["hValue"], NumberStyles.HexNumber));

                        accessMask = Int32.Parse((string)row["dsAccess"], NumberStyles.HexNumber);
                        previousAccessMask = UInt32.Parse((string)row["grAccess"], NumberStyles.HexNumber);
                    }
                    catch (FormatException e)
                    {
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
                ProcessID = pid,
                Handles = handleList
            };

            return Tasks.Task.StartTask(elevationModel);
        }

        #endregion


        #region INJECTION TASK DISPATCH

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

        #endregion




    }




}
