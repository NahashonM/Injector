using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.ProcessInterface
{
    interface IProcess
    {
        IntPtr  Open(IntPtr pid, Int32 desiredAccess);      // Get a handle to the process
        byte    Architecture();                             // Check if 64 or 32 bit process

        UInt32 PID();                                       // Get PID of the process

    }
}
