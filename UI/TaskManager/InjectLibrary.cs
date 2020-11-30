using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{
	class InjectLibrary : TaskManager.Task
	{
		public int processID;
		public string injectionMethod;
		public List<string> libraryFiles;

		public InjectLibrary(): base()
		{
			injectionMethod = "LoadLibrary";
			libraryFiles = new List<string>();
		}


		public override int StartTask()
		{
			throw new NotImplementedException();
		}
	}
}
