
using System.Windows.Forms;

namespace injector
{

    public partial class maingui
    {


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
            handleDataTable.Columns.Add("hValue", typeof(string));
            handleDataTable.Columns.Add("hType", typeof(string));
            handleDataTable.Columns.Add("hName", typeof(string));
            handleDataTable.Columns.Add("grAccess", typeof(string));
            handleDataTable.Columns.Add("dsAccess", typeof(string));

            //dtvFileSelections.RowsAdded += DataTablesRowAdded;
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
            dtvFileSelections.Columns[0].Frozen = true;

            dtvFileSelections.Columns[1].HeaderText = "Handle";
            dtvFileSelections.Columns[1].Resizable = DataGridViewTriState.False;
            dtvFileSelections.Columns[1].MinimumWidth = 50;
            dtvFileSelections.Columns[1].Width = 50;
            dtvFileSelections.Columns[1].Frozen = false;
            dtvFileSelections.Columns[1].ReadOnly = true;

            dtvFileSelections.Columns[2].HeaderText = "Type";
            dtvFileSelections.Columns[2].Resizable = DataGridViewTriState.True;
            dtvFileSelections.Columns[2].MinimumWidth = 60;
            dtvFileSelections.Columns[2].Width = 80;
            dtvFileSelections.Columns[2].Frozen = false;
            dtvFileSelections.Columns[2].ReadOnly = true;

            dtvFileSelections.Columns[3].HeaderText = "Name";
            dtvFileSelections.Columns[3].Resizable = DataGridViewTriState.True;
            dtvFileSelections.Columns[3].MinimumWidth = 120;
            dtvFileSelections.Columns[3].Width = 150;
            dtvFileSelections.Columns[3].Frozen = false;
            dtvFileSelections.Columns[3].ReadOnly = true;

            dtvFileSelections.Columns[4].HeaderText = "Access";
            dtvFileSelections.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[4].ReadOnly = true;
            dtvFileSelections.Columns[4].Frozen = false;


            dtvFileSelections.Columns[5].HeaderText = "N_Access";
            dtvFileSelections.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtvFileSelections.Columns[5].ReadOnly = false;
            dtvFileSelections.Columns[5].Frozen = false;
        }



    }


}
