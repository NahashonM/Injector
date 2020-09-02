using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{

    /// <summary>
    /// Dispatches the selected task based on the task model
    /// </summary>
    static partial class Task
    {
 
        private static void OnHandleReadEventListener(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null && args.Data.Length > 0)
                onHandleReadCallback(args.Data);
        }


        private static void OnMethodReadEventListener(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null && args.Data.Length > 0)
                onMethodReadCallback(args.Data);
        }
    }
}
