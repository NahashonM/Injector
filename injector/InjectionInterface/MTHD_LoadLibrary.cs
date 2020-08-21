using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace injector.Tasks.Methods
{
    
    internal class MTHD_LoadLibrary : IMethod
    {
        private const uint MEM_COMMIT_RESERVE = 0x1000 | 0x2000;
        private const uint PAGE_READWRITE = 0x04;


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string moduleName);


        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);


        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr VirtualAllocEx( IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flAllocationType, uint flProtect  );


        [DllImport("kernel32.dll", SetLastError =true ,CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, 
            string lpBuffer, UIntPtr nSize, ref UIntPtr lpNumberOfBytesWritten );

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize,
                    UIntPtr lpStartAddress, UIntPtr lpParameter,uint dwCreationFlags, ref IntPtr lpThreadId  );


        private IntPtr hProcess;

        private IntPtr hKernel32;
        private UIntPtr addrLoadLib;
        private UIntPtr pathAddress;

        private InjectionModel injectionModel ;

        private bool GetProcessHandle()
        {
            hProcess = IntPtr.Zero;

            if (injectionModel.HijackHandle)
            {
                // TODO--- Hijack handle no driver
                if (injectionModel.ObtainHandleViaDriver)
                {
                    // Driver dup handle
                }
                else
                {
                    // Dup an handle
                }
            }
            else
            {
                // TODO---- Get new handle
                if (injectionModel.ObtainHandleViaDriver)
                {

                }
                else
                {
                    hProcess = Natives.OpenProcess(Natives.ACCESS_MASK.PROCESS_ALL_ACCESS, false, injectionModel.TargetPid);
                }

            }

            if (hProcess.ToInt64() > 0)
                return true;

            return false;
        }




        public bool Inject(InjectionModel injectionModel)
        {
            this.injectionModel = injectionModel;

            if (GetProcessHandle())                                                 // get handle to process
            {
                GetLoadLibraryAddress();                                            // Get LoadLibrary remote address

                foreach (string file in injectionModel.FilesList)
                {
                    if (WritePathToProcess(file))                                   // Write dll path in remote process
                    {
                        // Inject Dll
                        IntPtr threadID = IntPtr.Zero;
                        IntPtr hhThread = CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, addrLoadLib,
                            pathAddress, 0, ref threadID);
                    }

                }


                Natives.CloseHandle(hProcess);                                      // Close handle to process
            }

            return false;
        }

        /// <summary>
        /// GetLoadLibraryAddress
        /// </summary>
        /// <returns></returns>
        private bool GetLoadLibraryAddress()
        {
            // Get address of kernel32.dll in remote process
            hKernel32 = GetModuleHandle("kernel32.dll");
            if (hKernel32.ToInt64() > 0)
            {
                string test = (Encoding.Default == Encoding.Unicode) ? "LoadLibraryW" : "LoadLibraryA";
                // Get address of loadlibrary function in remote module kernel32
                if ((UInt64)(addrLoadLib = GetProcAddress(hKernel32, (Encoding.Default == Encoding.Unicode) ? "LoadLibraryW": "LoadLibraryA")) > 0) 
                    return true;
            }
            return false;
        }

        /// <summary>
        /// WritePathToProcess
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool WritePathToProcess(string path)
        {
            pathAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (UIntPtr)path.Length + 2, MEM_COMMIT_RESERVE, PAGE_READWRITE);

            if (pathAddress.ToUInt64() > 0)
            {
                UIntPtr bytesWritten = UIntPtr.Zero;
                if (WriteProcessMemory(hProcess, pathAddress, path, (UIntPtr)path.Length + 1, ref bytesWritten))
                    return true;
            }

            return false;
        }





        public string GetName()
        {
            return "Load Library";
        }
        
        

    }
}
