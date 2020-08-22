using System;
using System.Runtime.InteropServices;

namespace injector
{
    public static partial class Natives
    {
        public delegate void EnumProcess_Callback(ProcessInfo processInfo);

        #region CONSTANTS
        /// <summary> DOS header signature at offset 0 </summary>
        private const int DOS_HEADER_VALUE_e_magic = 0x5A4D;

        /// <summary> PE header signature at offset +DOS_HEADER_e_lfanew</summary>
        private const uint PE_HEADER_VALUE_Signature = 0x4550;

        /// <summary>Image file value found at start of PE header that indicates assembly is 64bit. </summary>
        private const ushort PE_IMAGE_FILE_HEADER_Machine = 0x8664;


        /// <summary>The offset to PIMAGE_DOS_HEADER->e_lfanew </summary>
        private const int DOS_HEADER_OFFSET_e_lfanew = 0x3c;


        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;

        #endregion


        #region PROCESS_ENUMERATION_TYPES

        /// <summary> Snapshot of a system object
        ///    can be a heaplist, process, threads, all module or just 32bit modules </summary>
        private enum SNAPSHOT_TYPE : uint
        {
            TH32CS_SNAPHEAPLIST = 0x1,
            TH32CS_SNAPPROCESS = 0x2,
            TH32CS_SNAPTHREAD = 0x4,
            TH32CS_SNAPMODULE = 0x8,
            TH32CS_SNAPMODULE32 = 0x10
        }

        /// <summary> Process Architecture </summary>
        private enum IMAGE_FILE_MACHINE : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
        }

        /// <summary> Permissions needed when obtaining a handle to a process </summary>
        public enum ACCESS_MASK : uint
        {
            PROCESS_VM_READ = 0x10,
            PROCESS_VM_WRITE = 0x20,
            PROCESS_QUERY_INFORMATION = 0x400,
            PROCESS_SUSPEND_RESUME = 0x800,
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,
            PROCESS_ALL_ACCESS = 0x1FFFFF
        }

        /// <summary> Switch </summary>
        private enum ENUMERATION_MODE { FIND_ALL, FIND_BY_PID, FIND_BY_NAME }

        /// <summary> An entry in a snapshot 
        /// dwsize = 300bytes for ascii and 560 for unicode snapshots </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        private struct PROCESSENTRY32
        {
            const int MAX_PATH = 260;
            public UInt32 dwSize;                 // 0
            public UInt32 cntUsage;               // 4
            public UInt32 th32ProcessID;          // 8 this process
            public UIntPtr th32DefaultHeapID;      // 12
            public UInt32 th32ModuleID;           // 20 associated exe
            public UInt32 cntThreads;             // 24
            public UInt32 th32ParentProcessID;    // 28 this process's parent process
            public Int32 pcPriClassBase;         // 32 Base priority of process's threads
            public UInt32 dwFlags;                // 36
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PATH * 2)]
            public byte[] szExeFile;                // 40 Path
        }

        #endregion


        #region PRIVILEGE_TYPES_AND_ENUMERATIONS

        private enum Privilege_attrib
        {
            SE_PRIVILEGE_ENABLED = 0x00000002,
            SE_PRIVILEGE_REMOVED = 0x00000004
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public LUID Luid;
            public uint Attributes;
        }

        #endregion



        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
    }


}