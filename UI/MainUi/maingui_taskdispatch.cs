
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
        private bool DispatchElevationTask()
        {

			var tm = new Tasks.ElevateHandles();
			tm.processID = (int)SelectedProcess.Pid;

            foreach (DataRow row in handleDataTable.Rows)
            {
                if ((bool)row["elevate"] == true)
                {
                    UIntPtr handleValue = UIntPtr.Zero;
                    uint accessMask = 0;
                    uint previousAccessMask = 0;

                    try
                    {
                        handleValue = (UIntPtr)((IntPtr.Size == 4) ?
                                              UInt32.Parse((string)row["hValue"], NumberStyles.HexNumber) :
                                              UInt64.Parse((string)row["hValue"], NumberStyles.HexNumber));

                        accessMask = UInt32.Parse((string)row["dsAccess"], NumberStyles.HexNumber);
                        previousAccessMask = UInt32.Parse((string)row["grAccess"], NumberStyles.HexNumber);
                    }
                    catch (FormatException e)
                    {
                        continue;
                    }

					tm.handles.Add(handleValue);

					accessMask = (accessMask == 0) ? 0x01: accessMask;
					tm.newAccessMasks.Add( (uint)((accessMask == previousAccessMask) ? 0x1fffff : accessMask));
                }
            }


            if (tm.handles.Count < 1)
            {
                MessageBox.Show("Please Select handle(s) to elevate");
                return false;
            }

			TaskManager.StartTask(tm.taskID);

			return true;
        }

        #endregion


        #region INJECTION TASK DISPATCH

        /// <summary>
        /// Calls for dispatch of injection task
        /// </summary>
        /// <param name="pid"></param>
        private void DispatchInjectionTask()
        {
			var tm = new Tasks.InjectLibrary();
			tm.processID = (int)SelectedProcess.Pid;

            foreach (DataRow row in fileDataTable.Rows)
            {
				if ((bool)row["inject"] == true && (string)row["fileArch"] == lblArch.Text)
					tm.libraryFiles.Add(row["filePath"].ToString());
            }


            if (tm.libraryFiles.Count() < 1)
            {
                MessageBox.Show("Please Add files to inject");
                return;
            }

			TaskManager.StartTask(tm.taskID);

			return;
        }

		#endregion


		#region GET_HANDLES

		public void RefreshProcessHandles()
		{
			var tm = new Tasks.QueryHandles((int)SelectedProcess.Pid);
			tm.fnHandleFound = NewHandleFound_Callback;

			TaskManager.StartTask(tm.taskID);

		}
		#endregion // GET_HANDLES




	}




}
