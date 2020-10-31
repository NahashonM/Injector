#pragma once

#include <Windows.h>
#include <string>
#include <vector>


#include <iostream>



enum JOBTYPE {	Job_Inject, Job_Elevate, Job_Get_Process_Handles };


typedef struct _HANDLE_NEW_ACCESS_ {
	HANDLE		hValue;
	ACCESS_MASK desiredAccess;
}HANDLE_NEW_ACCESS;



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

	void		ParseHandleArguments(std::string rawString, PHANDLE hValue, ACCESS_MASK* desiredAccess);

public:

	std::vector<std::string>		filesList;
	std::vector<HANDLE_NEW_ACCESS>	handlesList;


	JOBDESCRIPTOR() : isValidJob(false) {}
	JOBDESCRIPTOR(const int argc, const char* argv[]) : isValidJob(false) { CommandLineParser(argc, argv); }

	void CommandLineParser(const int argc, const char* argv[]);

	/// <summary> Returs true if job is valid otherwise False </summary>
	/// <returns> [bool] </returns>
	bool IsValid();

	/// <summary> Returns the pid of the target process </summary>
	/// <returns> [uint32_t] </returns>
	uint32_t	Pid();

	/// <summary> Returns the type of job dispatched</summary>
	/// <returns> [JOBTYPE] </returns>
	JOBTYPE		JobType();

	/// <summary> Returns the type of job dispatched</summary>
	/// <returns> [bool] </returns>
	bool		HijackHandle();

	/// <summary> Returns true if the job requires handles to the taret to be elevated </summary>
	/// <returns> [bool] </returns>
	bool		ElevateHandle();

	/// <summary> Returns true if the job requires the driver to be unloaded once done, otherwise its unloaded </summary>
	/// <returns> [bool] </returns>
	bool		UnloadDriverOnInject();

	/// <summary> Returns true if the job requires use of the driver to obtain handles to target via driver.</summary>
	/// <returns> [bool] </returns>
	bool		ObtainHandleViaDriver();

	/// <summary> Returns true if the job requires use of the driver to obtain handles to target via driver.</summary>
	/// <returns> [std::string] </returns>
	std::string	InjectionMethod();

	/// <summary> Returns the number of files/handles specified in the job.</summary>
	/// <returns> int </returns>
	int ResourceCount();

	/// <summary> Returns a vector<sting> of files on the job.</summary>
	/// <returns>std::vector<std::string></returns>
	std::string		Files();

	/// <summary> Returns a HANDLE_NEW_ACCESS of handles.</summary>
	/// <returns> HANDLE_NEW_ACCESS</returns>
	HANDLE_NEW_ACCESS	Handle(int index);



	~JOBDESCRIPTOR();
};

