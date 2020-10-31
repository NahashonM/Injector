using System;
using System.Drawing;
using System.Text;

namespace injector
{
    public class ProcessInfo
    {

        private uint            pid;                            // Process id 
        private string          name;                           // Name of process

        private StringBuilder   path = new StringBuilder(260);  // Path of process image
        private string          arch;                           // Architecture of the process

        private Lazy<Image> lazyIcon;               // Process Icon


        public uint     Pid  {
            get { return pid; }
            set {
                pid = value;
                IntPtr hProcess = Natives.OpenProcess(
                                                Natives.ACCESS_MASK.PROCESS_QUERY_LIMITED_INFORMATION,
                                                false, (int)value
                                                    );

                if (hProcess != IntPtr.Zero)
                {
                    Natives.GetModuleFileNameEx(hProcess, (IntPtr)0, path, 260);
                    GetProcessArchitecture(hProcess);
                } else {
                    path.Clear();
                }

                Natives.CloseHandle(hProcess);
            }
        }


        public string Name {
            get { return name;  }
            set { name = value; }
        }


        public string   Path { get { return path.ToString(); } }
        public string   Arch { get { return arch; } }
        public Image    Icon { get { return lazyIcon.Value; } }
        

        public ProcessInfo() { }

        public ProcessInfo(uint pid, string name, string path, string arch)
        {
            this.pid = pid;
            this.name = name;
            this.path.Insert(0, path, 1);
            this.arch = arch;

            lazyIcon = new Lazy<Image>(() =>
            {
                return Natives.GetIconForFile(this.Path)?.ToBitmap();
            });
        }

        public ProcessInfo(uint pid, string name)
        {
            this.pid = pid;
            this.name = name;

            lazyIcon = new Lazy<Image> (() =>
            {
                return Natives.GetIconForFile(this.Path)?.ToBitmap();
            });
        }

        public void updateInfo(uint pid, string name)
        {
            Pid = pid;
            this.name = name;

            lazyIcon = new Lazy<Image>(() =>
            {
                return Natives.GetIconForFile(this.Path)?.ToBitmap();
            });
        }



        /// <summary>
        /// Get architecture of process binary
        /// </summary>
        /// <param name="hProcess">handle to the process</param>
        private void GetProcessArchitecture(IntPtr hProcess)
        {
            Natives.IMAGE_FILE_MACHINE processMachine = 0, nativeMachine = 0;

            if (Natives.IsWow64Process2(hProcess, ref processMachine, ref nativeMachine))
            {
                switch (processMachine)
                {
                    case Natives.IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_AMD64:
                        this.arch = "x86_64";
                        break;
                    case Natives.IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_I386:
                        this.arch = "x86";
                        break;
                    case Natives.IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_UNKNOWN:
                        switch (nativeMachine)
                        {
                            case Natives.IMAGE_FILE_MACHINE.IMAGE_FILE_MACHINE_AMD64:
                                this.arch = "x86_64";
                                break;
                            default:
                                this.arch = "x86";
                                break;
                        }
                        break;
                }
            } else {
                this.arch = "<error>";
            }
        }
    }
}
