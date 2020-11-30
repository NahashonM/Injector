using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{
	public class QueryHandles : TaskManager.Task
	{

		private int processID;

		public LibNatives.HANDLE_FOUND_CALLBACK		fnHandleFound = null;


		public QueryHandles(int processID) : base() { this.processID = processID; }

		
		public override int StartTask()
		{
			return LibNatives.GetProcessHandles(processID, fnHandleFound, fnErrorOccured);
		}

	}
}
