using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injector.Tasks
{
	class ElevateHandles : TaskManager.Task
	{
		public int				processID;
		public List<UIntPtr>	handles;
		public List<UInt32>		newAccessMasks;


		public ElevateHandles() : base()
		{
			handles = new List<UIntPtr>();
			newAccessMasks = new List<uint>();
		}


		public override int StartTask()
		{
			return LibNatives.ElevateProcessHandles(processID, handles.Count, handles.ToArray(),
													newAccessMasks.ToArray(), fnErrorOccured);
		}
	}
}
