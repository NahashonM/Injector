using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleValue">String representing handle params [handleValue;;;]</param>
    public delegate void OnHandleReadCallback(string handleValue);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleValue"></param>
    public delegate void OnMethodReadCallback(string handleValue);



    /// <summary>
    /// Job type
    /// </summary>
    public enum TASK_MODE : ushort
    {
        INJECTION = 0,
        ELEVATION = 1,
        QUERY_HANDLES = 2,
        QUERY_METHODS = 3,
    }




    /// <summary>
    /// Injection task description
    /// </summary>
    public struct InjectionModel
    {
        /// <summary> Pid of Process </summary>
        public int TargetPid;
        /// <summary> Architecture of Process </summary>
        public string TargetArchitecture;
        /// <summary> Injection Method to use </summary>
        public string InjectionMethod;
        public bool HijackHandle;
        public bool ElevateHandle;
        /// <summary>Use driver to obtain handles</summary>
        public bool ObtainHandleViaDriver;
        /// <summary>Unload driver once file(s) are injected</summary>
        public bool UnloadOnInject;
        /// <summary>List of path(s) to file(s) being injected </summary>
        public List<string> FilesList;
    }




    /// <summary>
    /// Handle elevation task description
    /// </summary>
    public struct ElevationModel
    {
        /// <summary> Pid of Process whose handle(s) is being elevated </summary>
        public int ProcessID;
        /// <summary> List of handle(s) to be elevated </summary>
        public List<uint> Handles;
    }




    static partial class Task
    {

        /// <summary>
        /// Generate commandline arguments fro injection task
        /// </summary>
        /// <param name="injectionModel">Injection task descriptor</param>
        /// <returns></returns>
        private static string GenerateCommandLineArguments(ref InjectionModel injectionModel)
        {
            string cmdArgs = "-t 0";                                                    // job Type
            cmdArgs += " -p " + injectionModel.TargetPid.ToString();                    // TargetPid
            cmdArgs += " -h " + ((injectionModel.HijackHandle) ? "1" : "0");             // HijackHandle [1 : true / 0 : false]
            cmdArgs += " -e " + ((injectionModel.ElevateHandle) ? "1" : "0");           // ElevateHandle [0 : true / 1 : false]
            cmdArgs += " -u " + ((injectionModel.ObtainHandleViaDriver) ? "1" : "0");   // UnloadDriverOnInject [0 : true / 1 : false]
            cmdArgs += " -o " + ((injectionModel.UnloadOnInject) ? "1" : "0");          // ObtainHandleViaDriver [0 : true / 1 : false]
            cmdArgs += " -m " + injectionModel.InjectionMethod;                         // InjectionMethod [string]
            cmdArgs += " -r ";                                                          // InjectionResources [csv list]

            foreach (string file in injectionModel.FilesList)
                cmdArgs += file + ",";

            return cmdArgs;
        }



        /// <summary>
        /// Generate commandline arguments for elevation task
        /// </summary>
        /// <param name="elevationModel">elevation task descriptor</param>
        /// <returns>parsed string</returns>
        private static string GenerateCommandLineArguments(ref ElevationModel elevationModel)
        {
            string cmdArgs = "-t 1";                                                    // job Type
            cmdArgs += " -p " + elevationModel.ProcessID.ToString();                    // TargetPid
            cmdArgs += " -r ";                                                          // InjectionResources [csv list]

            foreach (uint handle in elevationModel.Handles)
                cmdArgs += handle.ToString() + ",";

            return cmdArgs;
        }

    }
}
