using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks.Methods
{
    
    internal class MTHD_LoadLibrary : IMethod
    {
        private IntPtr hProcess;
        private InjectionModel injectionModel ;

        private bool GetProcessHandle()
        {
            hProcess = Natives.OpenProcess(Natives.ACCESS_MASK.PROCESS_ALL_ACCESS,false,pid);
            return false;
        }

        public bool Inject(InjectionModel injectionModel)
        {
            this.injectionModel = injectionModel;

            // get handle to process

            return false;
        }


        public string GetName()
        {
            return "Load Library";
        }
        
        

    }
}
