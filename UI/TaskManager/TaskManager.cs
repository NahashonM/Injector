using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace injector
{
	public static class TaskManager {

		public abstract class Task
		{
			public delegate void TASK_COMPLETED_CALLBACK(int exitStatus);

			public LibNatives.ERROR_OCCURED_CALLBACK fnErrorOccured = null;
			public TASK_COMPLETED_CALLBACK			 fnTaskCompleted = null;

			private Thread	_taskThread = null;
			private int		_taskID = -1;

			public int taskID { get { return _taskID; } }
			public Thread taskThread {
				get { return _taskThread; }
				set {
					for (int i = 0; i < TaskList.Count; i++){
						if (_taskID == TaskList[i].taskID)
						{
							_taskThread = value;
							break;
						}
						_taskThread = null;
					}
				}
			}


			/// <summary>
			/// Base method for all tasks.. 
			/// Adds each task to the task manager list
			/// </summary>
			protected Task() {
				if (TaskList.Count > 0)
					for (int i = 0; i < TaskList.Count; i++)
					{
						if (TaskList[i].taskID != TaskList.Count)
							_taskID = TaskList.Count;
						else
							_taskID = TaskList.Count + 1;
					}
				else
					_taskID = 0;

				TaskList.Add(this);
			}

			/// <summary>
			/// TaskManager Start task
			/// </summary>
			public abstract int StartTask();


			public void CancelTask()
			{
				if (_taskThread != null && _taskThread.IsAlive)
					_taskThread.Abort();
			}


			public void OnTaskCompleted(int exitCode)
			{
				TaskList.RemoveAt(taskID);
				if (fnTaskCompleted != null) fnTaskCompleted(exitCode);
			}
		
		}




		private static List<Task> TaskList;
		private static int taskInPool = 0;


		/// <summary>
		/// 
		/// </summary>
		static TaskManager() {
			TaskList = new List<Task>();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool StartTask(int id)
		{
			for (int i = 0; i < TaskList.Count; i++) { 
				if (TaskList[i].taskID == id) {

					if (TaskList[id].taskThread != null && TaskList[id].taskThread.IsAlive)
						return false;

					taskInPool = id;

					TaskList[id].taskThread = new Thread(ExecuteTask);

					TaskList[id].taskThread.IsBackground = true;
					TaskList[id].taskThread.Start();

					return true;

				}
			}
			return false;
		}
		
		/// <summary>
		/// 
		/// Concurrent access to the list causes errors here
		/// 
		/// </summary>
		private static void ExecuteTask()
		{
			int exitCode = TaskList[taskInPool].StartTask();

			TaskList[taskInPool].OnTaskCompleted(exitCode);
		}
	}
}
