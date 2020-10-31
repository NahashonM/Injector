using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        static OnHandleReadCallback onHandleReadCallback;
        static OnMethodReadCallback onMethodReadCallback;
        static OnTaskEndCallback    onTaskEndCallback;

        static TASK_MODE taskType;
        static bool      isTaskRunning = false;
        static Process   process;

        

        /// <summary>
        /// Injects the files into the specified process
        /// </summary>
        /// <param name="injectionModel">Injection parameters</param>
        /// <returns></returns>
        public static bool StartTask(InjectionModel injectionModel)
        {
            taskType = TASK_MODE.INJECTION;
            return ExecuteTask( GenerateCommandLineArguments(ref injectionModel),
                                ((injectionModel.TargetArchitecture == "x86") ? "x86" : "x64")
                               );
        }


        /// <summary>
        /// Elevate handle(s)
        /// </summary>
        /// <param name="elevationModel"></param>
        /// <returns></returns>
        public static bool StartTask(ElevationModel elevationModel)
        {
            taskType = TASK_MODE.ELEVATION;
            return ExecuteTask( GenerateCommandLineArguments(ref elevationModel), 
                                "x64"
                               );
        }


        /// <summary>
        /// Query Methods that are implimented
        /// </summary>
        /// <returns></returns>
        public static bool QueryProcessHandles(uint pid, OnHandleReadCallback handleReadListener, OnTaskEndCallback taskEndCallback)
        {
            Contract.Requires(handleReadListener != null);
            Contract.Requires(taskEndCallback != null);

            onTaskEndCallback = taskEndCallback;
            taskType = TASK_MODE.QUERY_HANDLES;

            onHandleReadCallback = handleReadListener;

            return ExecuteTask( "-t "+ ((ushort)TASK_MODE.QUERY_HANDLES).ToString() + " -p " + pid.ToString(),
                                "x64"   // Makes use of driver which only works with the x64 version
                               );
        }


        /// <summary>
        /// Query Methods that are implimented
        /// </summary>
        /// <returns></returns>
        public static bool QueryInjectionMethods(OnMethodReadCallback methodReadListener, OnTaskEndCallback taskEndCallback)
        {
            Contract.Requires(methodReadListener != null);
            Contract.Requires(taskEndCallback != null);

            onTaskEndCallback = taskEndCallback;
            taskType = TASK_MODE.QUERY_METHODS;

            onMethodReadCallback = methodReadListener;

            return ExecuteTask("-t " + ((ushort)TASK_MODE.QUERY_METHODS).ToString(),
                                "x64"   // Makes use of driver which only works with the x64 version
                               );
        }




        /// <summary>
        /// Dispatch task to side by side exe
        /// </summary>
        /// <param name="args">commandline arguments</param>
        /// <param name="outputHandler">[default:=null]</param>
        /// <param name="errorHandler">[default:=null]</param>
        /// <param name="targetArch">[default:=x64]</param>
        /// <returns></returns>
        private static bool ExecuteTask( string args, string targetArch = "x64")
        {
            process = new Process();
            
            process.StartInfo.FileName = "Injector_" + targetArch + ".exe";
            process.StartInfo.Arguments = args;

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            process.Exited += OnProcessExitedEventListener;

            process.OutputDataReceived += outputReadEventListener;
            process.ErrorDataReceived += errorReadEventListener;

            process.StartInfo.Verb = "runas";

            if (process.Start())
            {
                isTaskRunning = true;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                //process.WaitForExit();
                //process.Close();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Cancel running task
        /// </summary>
        /// <returns></returns>
        public static void CancelTask()
        {
            // Some tasks need not be canceled
            if(isTaskRunning && taskType == TASK_MODE.QUERY_HANDLES)
            {
                process.CancelOutputRead();
                process.CancelErrorRead();
                process.Close();

                isTaskRunning = false;
                onTaskEndCallback?.Invoke(taskType, 100);
            }
        }
    }
}
