using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector
{
    public class ProcessInfo
    {

        private uint   pid;                         // Process id 
        private string path;                        // Path of process image
        private string name;                        // Name of process
        private string arch;                        // Architecture of the process

        private Lazy<Image> lazyIcon;               // Process Icon

        public uint Pid { get { return pid; } set { pid = value; } }
        public string Path { get { return path; } set { path = value; } }
        public string Name { get { return name; } set { name = value; } }
        public Image Icon { get { return lazyIcon.Value; } }
        public string Arch { get { return arch; } }


        public ProcessInfo() { }

        public ProcessInfo(uint pid, string name, string path,string arch = "")
        {
            this.pid = pid;
            this.name = name;
            this.path = path;
            this.arch = arch;

            lazyIcon = new Lazy<Image> (() =>
            {
                return Natives.GetIconForFile(this.path)?.ToBitmap();
            });
        }

        public void updateInfo(uint pid, string name, string path, string arch = "")
        {
            this.pid = pid;
            this.name = name;
            this.path = path;
            this.arch = arch;

            lazyIcon = new Lazy<Image>(() =>
            {
                return Natives.GetIconForFile(this.path)?.ToBitmap();
            });
        }
    }


    /*
     * public class ProcessInfo
    {

        public uint   Pid { get; }                        // Process id 
        public string Path { get; }                       // Path of process image
        public string Name { get; }                       // Name of process
        public string Arch { get; }                       // Architecture of the process
        public Image  Icon => lazyIcon.Value;             // Process Icon


        private readonly Lazy<Image> lazyIcon;

        public ProcessInfo(uint pid, string name, string path,string arch = "")
        {
            Pid = pid;
            Name = name;
            Path = path;
            Arch = "x86"; //arch;

            lazyIcon = new Lazy<Image> (() =>
            {
                return Natives.NativeFn.GetIconForFile(Path)?.ToBitmap();
            });
        }

        public void UpdateInfo(uint pid, string name, string path, string arch = "")
        {
            Pid = pid;
            Name = name;
            Path = path;
            Arch = "x86"; //arch;

            lazyIcon = new Lazy<Image>(() =>
            {
                return Natives.NativeFn.GetIconForFile(Path)?.ToBitmap();
            });
        }
    }
     */
}
