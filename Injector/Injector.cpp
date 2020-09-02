#include <iostream>

#include "JobDescriptor.h"
#include "kprocesshacker.h"
#include "jobHandlers.h"



int main(const int argc, const char* argv[] )
{
	// Process Commandline arguments
	//--------------------------------
	JOBDESCRIPTOR job(argc, argv);
	
	// Get debugging privilege
	//--------------------------------
	if (util::SetPrivilege(SE_DEBUG_NAME, true)) {
	}
	//std::cout << "Debug privileges obtained" << std::endl;


	// Dispatch job to handler
	//-----------------------------------------------------------

	if (job.IsValid())
	{
		//std::cout << "Job is valid" << std::endl;

		switch (job.JobType())
		{
		case Job_Elevate:
			break;
		case Job_Inject:
			break;
		case Job_Get_Process_Handles:
			GetProcessHandles(&job);
			break;
		}
	} else {
		//std::cout << "Job is not valid" << std::endl;
	}
}

