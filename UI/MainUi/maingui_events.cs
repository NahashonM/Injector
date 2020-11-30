
using System;
using System.Data;
using System.Windows.Forms;

namespace injector
{

    public partial class maingui
    {

        private bool windowDragEvt = false;                // Custom title bar mouse onmove event tracker
        private int mouseX, mouseY;                        // Custom title bar Mouse position trackers




        
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

            try
            {
                Int32.Parse(lblPid.Text.Substring(0, lblPid.Text.IndexOf(' ')));
                Properties.Settings.Default.previousProcessName = lblName.Text;
            }
            catch (Exception)
            {
                Properties.Settings.Default.previousProcessName = null;
            }

            string files = null;
            foreach (DataRow row in fileDataTable.Rows)
            {
                files += row["filePath"].ToString() + ";" + row["inject"].ToString() + "\n";
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
                btnRefreshHandles.Visible = false;
            }
            else
            {
                SwitchDataTable(APP_MODE.HandleElevation);

                btnInject.Text = "Elevate Handle";
                btnAddFile.Visible = false;
                btnDeleteFile.Visible = false;
                btnRefreshHandles.Visible = true;
            }
        }



        #region BUTTON_EVENTS

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



        /// <summary>
        /// Refresh handle list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshHandles_Click(object sender, EventArgs e)
        {
            handleDataTable.Clear();
            btnRefreshHandles.Enabled = false;

			RefreshProcessHandles();

			btnRefreshHandles.Enabled = true;

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
        /// Dispatch injection or elevation task to the task handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInject_Click(object sender, EventArgs e)
        {
			if (SelectedProcess.Pid > 0) {
				if (rdInjectionMode.Checked)
					DispatchInjectionTask();
				else
					DispatchElevationTask();

				return;
			}

			MessageBox.Show("Please Select a process to inject into");
			return;
        }


        #endregion


        #region CUSTOM_TITLEBAR

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
        #endregion


        #region FILE_DRAG_DROP_EVENTS

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

        #endregion


        

    }



  
}
