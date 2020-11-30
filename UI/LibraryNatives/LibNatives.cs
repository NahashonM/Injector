using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace injector
{
    public static class LibNatives
    {



		/// <summary>
		/// Called when an error occurs during task processing.
		/// </summary>
		/// <param name="errorCode">[In] number describing the error code</param>
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public delegate void ERROR_OCCURED_CALLBACK([In] UInt16 errorCode);


		/// <summary>
		/// Called each time an handle is found. Used only if getting handles via 'GetProcessHandles'
		/// </summary>
		/// <param name="handle">[In] The value</param>
		/// <param name="accessMask">[In] Permission granted to the handle</param>
		/// <param name="type">[In] System Object type name of the handle</param>
		/// <param name="name">[In] Name value of the handle</param>
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet =CharSet.Unicode)]
        public delegate void HANDLE_FOUND_CALLBACK(	[In] UIntPtr handle,
													[In] UInt32 accessMask,
													[In] [MarshalAs(UnmanagedType.LPWStr)]string type, 
													[In] [MarshalAs(UnmanagedType.LPWStr)]string name);



#if x86
        const string libName = "LibInjector_x86.dll";
#elif x64
        const string libName = "LibInjector_x64.dll";
#endif


		/// <summary>
		/// 
		/// </summary>
		/// <param name="procPid"></param>
		/// <param name="fnHandleFound"></param>
		/// <param name="fnTaskStarted"></param>
		/// <param name="fnErrorOccured"></param>
		/// <param name="fnTaskFinished"></param>
		/// <returns></returns>
        [DllImport(libName, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetProcessHandles(    int procPid, 
                                                        HANDLE_FOUND_CALLBACK fnHandleFound,
														ERROR_OCCURED_CALLBACK fnErrorOccured = null
													);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetPid"></param>
		/// <param name="handleCount"></param>
		/// <param name="handles"></param>
		/// <param name="newAccessMasks"></param>
		/// <param name="fnTaskStarted"></param>
		/// <param name="fnErrorOccured"></param>
		/// <param name="fnTaskFinished"></param>
		/// <returns></returns>
        [DllImport(libName, CallingConvention = CallingConvention.StdCall)]
        public static extern int ElevateProcessHandles(int targetPid,
														int handleCount,
														[In] UIntPtr[] handles,
														[In] UInt32[] newAccessMasks,
														ERROR_OCCURED_CALLBACK fnErrorOccured = null
														);



		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public delegate void testFNNNNN([In] UInt64 svalue);


		[DllImport(libName, CallingConvention = CallingConvention.StdCall)]
		public static extern bool InjectLibraries(	[In] int count,
													[In] UIntPtr[] handles,
													[In] testFNNNNN fn
												);


	}
}
