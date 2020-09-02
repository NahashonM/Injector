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
        static OnHandleReadCallback onHandleReadCallback;
        static OnMethodReadCallback onMethodReadCallback;


        /// <summary>
        /// Injects the files into the specified process
        /// </summary>
        /// <param name="injectionModel">Injection parameters</param>
        /// <returns></returns>
        public static bool StartTask(InjectionModel injectionModel)
        {
            return ExecuteTask( GenerateCommandLineArguments(ref injectionModel),
                                null,
                                null,
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
            return ExecuteTask( GenerateCommandLineArguments(ref elevationModel), 
                                null, 
                                null, 
                                "x64"
                               );
        }


        /// <summary>
        /// Query Methods that are implimented
        /// </summary>
        /// <returns></returns>
        public static bool QueryProcessHandles(uint pid, OnHandleReadCallback handleReadListener)
        {
            onHandleReadCallback = handleReadListener;

            return ExecuteTask( "-t 2 -p " + pid.ToString(),
                                OnHandleReadEventListener,
                                null,
                                "x64"   // Makes use of driver which only works with the x64 version
                               );
        }


        /// <summary>
        /// Query Methods that are implimented
        /// </summary>
        /// <returns></returns>
        public static bool QueryInjectionMethods(OnMethodReadCallback methodReadListener)
        {
            onMethodReadCallback = methodReadListener;

            return ExecuteTask("-t 3 ",
                                OnMethodReadEventListener,
                                null,
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
        private static bool ExecuteTask( string args, 
            DataReceivedEventHandler outputHandler = null, 
            DataReceivedEventHandler errorHandler = null, 
            string targetArch = "x64")
        {
            Process process = new Process();
            process.StartInfo.FileName = "Injector_" + targetArch + ".exe";
            process.StartInfo.Arguments = args;

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            if (outputHandler!= null)   process.OutputDataReceived += outputHandler;
            if (errorHandler != null)   process.ErrorDataReceived += errorHandler;

            process.StartInfo.Verb = "runas";

            if (process.Start())
            {
                if (outputHandler != null) process.BeginOutputReadLine();
                if (errorHandler  != null) process.BeginErrorReadLine();

                process.WaitForExit();
                process.Close();

                return true;
            }

            return false;
        }


    }
}
