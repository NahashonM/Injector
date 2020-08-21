using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{
    /*  
     *  Finding the Methods should be resolved using naming convenctions of the methods
     *     The following aming rules apply
     *          1. The method name should start with MTHD_ [can be solved by implementing all on a new namespace]
     *          2. Any spaces in the display of the method name should be replaced with _
     *          3. 
     *          
     *  LoadLibrary Method / Code Caves / CreateRemoteThreadEx / 
     *                       ZwCreateThread / NtCreateThreadEx / RtlCreateUserThreadEx
     *  SetWindowsHookEx Method
     *  Registry Keys => [AppInitDlls/ KnownDlls]
     *  Image File Execution Options Key(IFEO)
     *  Kernel drivers
     *  AppCompat shimming layer
     *  
     *  APC (Asynchronous Procedure Calls)
     *  Reflective dll injection
     *  
     */
    internal interface IMethod
    {
        bool Inject(InjectionModel injectionModel);

        string GetName();
    }
}
