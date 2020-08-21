using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;



namespace injector
{

    public static partial class Natives
    {

        #region PUBLIC_IMPORTS_FROM_KERNEL32

        /// <summary>
        /// Win32Api OpenProcess.. Returns a handle to the process or INVALID_HANDLE_VALUE(-1) if unsuccsessul or 0
        /// </summary>
        /// <param name="dwDesiredAccess">Permissions needed</param>
        /// <param name="bInheritHandle">Whether the handle is inheritable by children</param>
        /// <param name="dwProcessId">target process id</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr OpenProcess(ACCESS_MASK dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Load a dynamic library into current process
        /// </summary>
        /// <param name="libraryPath">path to the library file</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string libraryPath);

        /// <summary>
        /// Free library and unload it from current process
        /// </summary>
        /// <param name="hModule">handle to the library</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Gets the address of an exported function
        /// </summary>
        /// <param name="hModule">handle to the library module</param>
        /// <param name="procName">name of the exported function</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Close an handle
        /// </summary>
        /// <param name="hObject">handle value</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern bool CloseHandle(IntPtr hObject);



        #endregion



        /*
         * **********************************************************************
         * **********************************************************************
         */

        /// <summary>
        /// Get the Icon of a Process image
        /// </summary>
        /// <param name="path">Path to the executable image of the process</param>
        /// <returns>true if succeded false on fail</returns>
        public static Icon GetIconForFile(string path)
        {
            var shinfo = new SHFILEINFO();
            if (SHGetFileInfo(path, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON) != (IntPtr)0)
            {
                var icon = Icon.FromHandle(shinfo.hIcon).Clone() as Icon;
                DestroyIcon(shinfo.hIcon);
                return icon;
            }

            return null;
        }

        /// <summary>
        /// Adjust Process Privileges only one at a time
        /// </summary>
        /// <param name="privilegeName">Name of the priviledge</param>
        /// <param name="enable">true: enable , false: disable</param>
        /// <returns>true: if succeeded else false</returns>
        public static bool SetPrivilege(string privilegeName, bool enable)
        {
            bool status = false;
            if (OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle, TokenAccessLevels.AllAccess, out var hToken))
            {
                var privileges = new TOKEN_PRIVILEGES
                {
                    PrivilegeCount = 1,
                    Luid = { LowPart = 0x0, HighPart = 0x0 },
                    Attributes = (enable) ? (uint)Privilege_attrib.SE_PRIVILEGE_ENABLED : (uint)Privilege_attrib.SE_PRIVILEGE_REMOVED
                };

                if (LookupPrivilegeValueA("", privilegeName, ref privileges.Luid))
                    if (AdjustTokenPrivileges(hToken, false, ref privileges, 0, IntPtr.Zero, IntPtr.Zero))
                        status = true;

                CloseHandle(hToken);
            }

            return status;
        }

        /// <summary>
        /// Find a process by its pid
        /// </summary>
        /// <param name="enumCallBack">callback method when the process is found</param>
        /// <param name="dwProcessID"> pid of process</param>
        /// <returns></returns>
        public static bool FindProcessByPID(EnumProcess_Callback enumCallBack, uint dwProcessID)
        {
            return EnumerateProcess(enumCallBack, dwProcessID, "", ENUMERATION_MODE.FIND_BY_PID);
        }

        /// <summary>
        /// Find a process by its name
        /// </summary>
        /// <param name="enumCallBack">callback method when the process is found</param>
        /// <param name="strProcName"> name of process</param>
        /// <returns></returns>
        public static bool FindProcessByName(EnumProcess_Callback enumCallBack, string strProcName)
        {
            return EnumerateProcess(enumCallBack, 0, strProcName, ENUMERATION_MODE.FIND_BY_NAME);
        }

        /// <summary>
        /// Get list of all processes currently running
        /// </summary>
        /// <param name="enumCallBack">callback method when each process is found</param>
        /// <returns></returns>
        public static bool GetListOfRunningProcesses(EnumProcess_Callback enumCallBack)
        {
            return EnumerateProcess(enumCallBack, 0, "", ENUMERATION_MODE.FIND_ALL);
        }

        /// <summary>
        /// Get architecture of a library file
        /// </summary>
        /// <param name="imagePath">Path to the library file</param>
        /// <returns></returns>
        public static string GetImageArchitecture(string imagePath)
        {
            string res = "";

            FileStream fStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            BinaryReader bReader = new BinaryReader(fStream);

            ushort dos_e_magic = bReader.ReadUInt16();

            fStream.Seek(DOS_HEADER_OFFSET_e_lfanew, SeekOrigin.Begin);
            uint dos_e_lfanew = bReader.ReadUInt32();

            fStream.Seek(dos_e_lfanew, SeekOrigin.Begin);
            uint pe_Signature = bReader.ReadUInt32();

            IMAGE_FILE_MACHINE machineType = (IMAGE_FILE_MACHINE)bReader.ReadUInt16();

            bReader.Close();
            fStream.Close();

            switch (machineType)
            {
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_UNKNOWN:
                    res = "Unknown";
                    break;
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_I386:
                    res = "x86";
                    break;
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_IA64:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_AMD64:
                    res = "x86_64";
                    break;
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_AM33:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_ARM:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_EBC:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_M32R:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_MIPS16:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_MIPSFPU:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_MIPSFPU16:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_POWERPC:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_POWERPCFP:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_R4000:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_SH3:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_SH3DSP:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_SH4:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_SH5:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_THUMB:
                case IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_WCEMIPSV2:
                    res = "__";
                    break;
            }


            return res;
        }
    }

}