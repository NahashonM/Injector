using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{
    public enum TASK_MODE : ushort
    {
        INJECTION = 0,
        ELEVATION = 1
    }


    /// <summary>
    /// Injection task description
    /// </summary>
    public struct InjectionModel
    {
        /// <summary> Pid of Process </summary>
        public int          TargetPid;
        /// <summary> Injection Method to use </summary>
        public string       InjectionMethod;
        public bool         HijackHandle;
        public bool         ElevateHandle;
        /// <summary>Use driver to obtain handles</summary>
        public bool         ObtainHandleViaDriver;
        /// <summary>Unload driver once file(s) are injected</summary>
        public bool         UnloadOnInject;
        /// <summary>List of path(s) to file(s) being injected </summary>
        public List<string> FilesList;
    }

    /// <summary>
    /// Handle elevation task description
    /// </summary>
    public struct ElevationModel
    {
        /// <summary> Pid of Process whose handle(s) is being elevated </summary>
        public int        ProcessID;
        /// <summary> List of handle(s) to be elevated </summary>
        public List<uint> Handles;
    }


    /// <summary>
    /// Dispatches the selected task based on the task model
    /// </summary>
    class Task
    {
        static IMethod selectedMethod;

        static Task()
        {
            selectedMethod = null;
        }

        /// <summary>
        /// Injects the files into the specified process
        /// </summary>
        /// <param name="injectionModel">Injection parameters</param>
        /// <returns></returns>
        public static bool StartTask(InjectionModel injectionModel)
        {
            string methodClass = "injector.Tasks.Methods.MTHD_" + injectionModel.InjectionMethod.Replace(' ', '_');
            Type type = Type.GetType(methodClass);
            selectedMethod = (IMethod)Activator.CreateInstance(type);

            selectedMethod.Inject(injectionModel);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elevationModel"></param>
        /// <returns></returns>
        public static bool StartTask(ElevationModel elevationModel)
        {

            return true;
        }


        /// <summary>
        /// Query Methods that are implimented
        /// </summary>
        /// <returns></returns>
        public static void QueryAvailableInjectionMethods(ref List<string> InjectionMethods)
        {
            foreach (Type itype in Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.Name.Contains("MTHD_")).ToList())
                InjectionMethods.Add(itype.Name.Substring(5).Replace('_', ' '));
        }
    }
}
