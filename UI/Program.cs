using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace injector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get Debug Privileges
            if (!Natives.SetPrivilege("SeDebugPrivilege", true))
            {
                MessageBox.Show("Failed to obtain Debug Privileges! Exiting..");
                return;
            }


            if ( InitProgram())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new maingui());
            }
        }

        static bool InitProgram ()
        {
            return true;
        }
    }
}
