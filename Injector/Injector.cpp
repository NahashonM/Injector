#include <iostream>

#include "JobDescriptor.h"


int main(const int argc, const char* argv[] )
{

	// Process Commandline arguments
	//-----------------------------------------------------------
	JOBDESCRIPTOR job(argc, argv);

	if (job.IsValid())
	{
		std::cout << "Job is valid";
	}
	else
	{
		return 1;
	}


}

