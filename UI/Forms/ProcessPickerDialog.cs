using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace injector.Forms
{
    public delegate void ProcessPicked_Callback(ProcessInfo selectedProcess);

    public partial class ProcessPickerDialog : Form
    {
        private ProcessPicked_Callback processPicked_Callback;

        private DataTable processData;
        private bool windowDragEvt = false;
        private int mouseX;
        private int mouseY;

        public ProcessPickerDialog()
        {
            InitializeComponent();

            processData = new DataTable();

            processData.Columns.Add("icon", typeof(Image));
            processData.Columns.Add("procPID", typeof(uint));
            processData.Columns.Add("procName", typeof(string));
            processData.Columns.Add("procModulePath", typeof(string));
            processData.Columns.Add("arch", typeof(string));

            procListView.DataSource = processData;

            SetProcessListViewStyle();
        }

        
        public ProcessPicked_Callback selected_Callback {
            get { return processPicked_Callback;  }
            set { processPicked_Callback = value; }
        }


        private void SetProcessListViewStyle()
        {
            procListView.AutoGenerateColumns = false;

            procListView.Columns[0].HeaderText = "";
            procListView.Columns[0].Resizable = DataGridViewTriState.False;
            procListView.Columns[0].Width = 25;

            procListView.Columns[1].HeaderText = "Pid";
            procListView.Columns[1].Resizable = DataGridViewTriState.False;
            procListView.Columns[1].Width = 60;

            procListView.Columns[2].HeaderText = "Process Name";
            procListView.Columns[2].MinimumWidth = 100;
            procListView.Columns[2].Width = 160;

            procListView.Columns[3].HeaderText = "Path";
            procListView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            procListView.Columns[4].Visible = false;
        }

        public void ProcessEnumeration_Callback(ProcessInfo processInfo)
        {
            DataRow row = processData.NewRow();

            row["icon"] = processInfo.Icon;
            row["procPID"] = processInfo.Pid;
            row["procName"] = processInfo.Name;
            row["procModulePath"] = processInfo.Path;
            row["arch"] = processInfo.Arch;
            
            processData.Rows.Add(row);
        }


        private void RefreshProcesses()
        {
            processData.Clear();
            Natives.EnumProcess_Callback enumCallBack = ProcessEnumeration_Callback;

            if (!Natives.GetListOfRunningProcesses(enumCallBack))
            {
                MessageBox.Show("Cannot Get running process..!\nPlease check if you have admin privs..!");
                this.Close();
            }

        }

        public DataRow SelectedRow =>
            (procListView.SelectedRows.Cast<DataGridViewRow>().
            FirstOrDefault()?.DataBoundItem as DataRowView)
            ?.Row;
            

        private bool ProcessCallbackData()
        {
            //Todo Process data and exit

            DataRow row = SelectedRow;
            if (row == null)
            {
                MessageBox.Show("You have not selected any process..!");
                return false;
            }

            ProcessInfo SelectedProcess = new ProcessInfo
                (
                (uint)row?.Field<uint>("procPID"),
                row?.Field<string>("procName"),
                row?.Field<string>("procModulePath"),
                row?.Field<string>("arch")
                );

            processPicked_Callback(SelectedProcess);

            return true;
        }



        private void CustomTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            windowDragEvt = true;
            mouseX = MousePosition.X;
            mouseY = MousePosition.Y;
        }

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

        private void CustomTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            windowDragEvt = false;
        }



        private void ProcessPickerDialog_Load(object sender, EventArgs e)
        {
           RefreshProcesses();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  
        private void TxtFilterString_TextChanged(object sender, EventArgs e)
        {
            var filter = txtFilterString.Text;

            if (!string.IsNullOrEmpty(filter))
            {
                filter = $"procName like '%{filter}%' or procModulePath like '%{filter}%'";
            }
           
            ((DataTable)procListView.DataSource).DefaultView.RowFilter = filter;
        }


        private void BtnRefreshProcesses_Click(object sender, EventArgs e)
        {
            RefreshProcesses();
        }

        private void BtnSelectProcess_Click(object sender, EventArgs e)
        {
            if (ProcessCallbackData())
                Close();
        }

        private void ProcListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if(ProcessCallbackData())
                    Close();
            }
        }
    }
}
