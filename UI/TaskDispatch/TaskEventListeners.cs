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

        private static void outputReadEventListener(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null && args.Data.Length > 0)
            {
                if (args.Data == "Begin") {
                
                }else if(args.Data == "End") {
                    process.CancelOutputRead();
                    process.CancelErrorRead();
                    process.Close();
                    
                    isTaskRunning = false;
                    onTaskEndCallback?.Invoke(taskType, 100);
                } else {
                    if(taskType == TASK_MODE.QUERY_HANDLES)
                        onHandleReadCallback(args.Data);
                    else if(taskType == TASK_MODE.QUERY_METHODS)
                        onMethodReadCallback(args.Data);
                }
            }
        }




        private static void errorReadEventListener(object sender, DataReceivedEventArgs args)
        {

                
        }


        private static void OnProcessExitedEventListener(object sender, EventArgs e)
        {
            if (isTaskRunning)
            {
                isTaskRunning = false;
                process.CancelOutputRead();
                process.CancelErrorRead();
                onTaskEndCallback?.Invoke(taskType, 100);
            }

            
        }
    }
}
