using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace injector
{
    public static partial class Natives
    {

        #region PRIVATE_IMPORTS_FROM_KERNEL32_AND_PSAPI

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CreateToolhelp32Snapshot([In]SNAPSHOT_TYPE dwFlags, [In]int th32ProcessID);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        private static extern bool Process32First([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        private static extern bool Process32Next([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        #endregion

        #region PRIVATE_IMPORTS_FROM_ADVAPI32

        [DllImport("advapi32.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccessLevels DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool LookupPrivilegeValueA(string lpSystemName, string lpName, ref LUID lpLuid);

        [DllImport("advapi32.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Zero, IntPtr Null1, IntPtr Null2);

        #endregion

        #region PRIVATE_IMPORTS_SHELL32
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int DestroyIcon(IntPtr hIcon);

        #endregion

        /*
         * **********************************************************************
         * **********************************************************************
         */


        /// <summary> Enumerate all or find a processe(s) in the system.
        /// enumCallBack is called every time process is found </summary>
        /// <param name="enumCallBack">Delegate to callback method called each time a process is found</param>
        /// <param name="mode">Working switch:: Find_ALL=Gets all process, FindByPid, FindByName</param>
        /// <param name="procPID">PID of process in FIND_BY_PID; 0 if in FIND_ALL mode</param>
        /// <param name="procName">Name of process in FIND_BY_NAME; empty string if in FIND_ALL mode</param>
        /// <returns></returns>
        private static bool EnumerateProcess(EnumProcess_Callback enumCallBack, UInt32 procPID = 0, string procName = "", ENUMERATION_MODE mode = ENUMERATION_MODE.FIND_ALL)
        {
            Contract.Requires(enumCallBack != null);

            bool findMode = (mode != ENUMERATION_MODE.FIND_ALL) ? true : false;
            bool findByName = (mode == ENUMERATION_MODE.FIND_BY_NAME) ? true : false;
            bool status = false;

            if (findMode || findByName) procPID = 0;

            IntPtr hSnap = CreateToolhelp32Snapshot(SNAPSHOT_TYPE.TH32CS_SNAPPROCESS, (int)procPID);        // Take Process Snapshot

            if (hSnap != IntPtr.Zero)                                                                       // Validate handle to snapshot
            {
                PROCESSENTRY32 entry        = new PROCESSENTRY32 { };
                entry.dwSize                = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

                ProcessInfo processInfo = new ProcessInfo();

                bool validEntry = Process32First(hSnap, ref entry);                    // Get first entry in the snapshot

                if (!findMode && validEntry) status = true;                            // Atleast 1 entry found in Find_All mode

                while (validEntry)
                {
                                                                            
                    string pName = Encoding.ASCII.GetString(entry.szExeFile).TrimStart('\0');   // Get Name of current snapshot entry
                    pName = pName.Substring(0, pName.IndexOf('\0'));

                    if (findMode)                                                                // Search Mode
                    {
                        if (findByName) {
                            if (pName.ToLower().Contains(procName.ToLower()))  {                                // search by name
                                validEntry = Process32Next(hSnap, ref entry); status = true;
                                continue;
                            }
                        }  else  {
                            if (entry.th32ProcessID != procPID) {                                               // Search by PID
                                validEntry = Process32Next(hSnap, ref entry); status = true;
                                continue;
                            }
                        }
                    }

                    processInfo.updateInfo(entry.th32ProcessID, pName);
                    enumCallBack(processInfo);                                                                   // Call the callback

                    validEntry = Process32Next(hSnap, ref entry);                                               // Get next entry in the snapshot
                }
            }

            CloseHandle(hSnap);
            return status;
        }

    }
}
