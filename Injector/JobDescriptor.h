#pragma once

#include <Windows.h>
#include <string>
#include <vector>


#include <iostream>



enum JOBTYPE {	Job_Inject, Job_Elevate, Job_Get_Process_Handles };

struct InjectionModel
{
	
	bool         UnloadOnInject;
	/// <summary>List of path(s) to file(s) being injected </summary>
	std::vector<std::string> FilesList;
};


class JOBDESCRIPTOR
{
private:
	bool		isValidJob;
	JOBTYPE		jobType;

	uint32_t	targetPid;				// -p <summary> Pid of Process </summary>
	bool		hijackHandle;			// -h <summary> Use driver to obtain handles</summary>
	bool		elevateHandle;			// -e <summary> Use driver to obtain handles</summary>
	bool		unloadDriverOnInject;	// -u <summary> Unload driver once file(s) are injected</summary>
	bool		obtainHandleViaDriver;	// -o <summary> Use driver to obtain handles</summary>

	std::string	injectionMethod;		// -m <summary> Injection Method to use </summary>

	void*		injectionResources;		// -r <summary>List of path(s) to file(s) to be injected or handles to  be elevated </summary>



	bool		ParseResourceValues(std::string rawString);

public:
	JOBDESCRIPTOR() : isValidJob(false) {};
	JOBDESCRIPTOR(const int argc, const char* argv[]) : isValidJob(false)  { CommandLineParser(argc, argv); }

	void CommandLineParser(const int argc, const char* argv[]);

	/// <summary> Returs true if job is valid otherwise False </summary>
	/// <returns> [bool] </returns>
	bool IsValid() { return isValidJob; }

	/// <summary> Returns the pid of the target process </summary>
	/// <returns> [uint32_t] </returns>
	uint32_t	Pid() { return targetPid; };

	/// <summary> Returns the type of job dispatched</summary>
	/// <returns> [JOBTYPE] </returns>
	JOBTYPE		JobType() { return jobType; };

	/// <summary> Returns the type of job dispatched</summary>
	/// <returns> [bool] </returns>
	bool		HijackHandle() { return hijackHandle; };

	/// <summary> Returns true if the job requires handles to the taret to be elevated </summary>
	/// <returns> [bool] </returns>
	bool		ElevateHandle() { return elevateHandle; };

	/// <summary> Returns true if the job requires the driver to be unloaded once done, otherwise its unloaded </summary>
	/// <returns> [bool] </returns>
	bool		UnloadDriverOnInject() { return unloadDriverOnInject; };

	/// <summary> Returns true if the job requires use of the driver to obtain handles to target via driver.</summary>
	/// <returns> [bool] </returns>
	bool		ObtainHandleViaDriver() { return obtainHandleViaDriver; };

	/// <summary> Returns true if the job requires use of the driver to obtain handles to target via driver.</summary>
	/// <returns> [std::string] </returns>
	std::string	InjectionMethod() { return injectionMethod;  };

	/// <summary> Returns a vector of files or handles depending on the job.</summary>
	/// <returns> [void*] casting to [std::vector<HANDLE>] or [std::vector<std::string>] may be needed</returns>
	void*		InjectionResources() { return injectionResources;  }


	~JOBDESCRIPTOR();
};

