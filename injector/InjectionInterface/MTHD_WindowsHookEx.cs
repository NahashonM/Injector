using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks.Methods
{
    internal class MTHD_WindowsHookEx : IMethod
    {
        public bool Inject(InjectionModel injectionModel)
        {

            return false;
        }

        public string GetName()
        {
            return "Windows Hook ex";
        }

    }
}
