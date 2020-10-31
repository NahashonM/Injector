#include "JobDescriptor.h"



JOBDESCRIPTOR::~JOBDESCRIPTOR()
{
}


/// <summary> Passes commandline arguments to construct a valid injection/elevation job </summary>
/// <param name="argc"> Commandline arguments count </param>
/// <param name="argv"> Commandline arguments </param>
/// <returns>Void</returns>
void JOBDESCRIPTOR::CommandLineParser(const int argc, const char* argv[])
{
	int rawResourceIndex = -1;

	for (int i = 1; i < argc; i++) {

		if (argv[i][0] != '-')	break;						// Wrong parameter

		char arg = tolower(argv[i][1]);

		if (arg == 't')											// Job type
		{
			jobType = (JOBTYPE)atoi(argv[i + 1]);
		}
		else if (arg == 'p')									// target Pid
		{
			targetPid = atoll(argv[i + 1]);
		}
		else if (arg == 'h')									// HijackHandle [0 : true / 1 : false]
		{
			hijackHandle = atoi(argv[i + 1]) ? true : false;
		}
		else if (arg == 'e')									// ElevateHandle [0 : true / 1 : false]
		{
			elevateHandle = atoi(argv[i + 1]) ? true : false;
		}
		else if (arg == 'u')									// UnloadDriverOnInject [0 : true / 1 : false]
		{
			unloadDriverOnInject = atoi(argv[i + 1]) ? true : false;
		}
		else if (arg == 'o')									// ObtainHandleViaDriver [0 : true / 1 : false]
		{
			hijackHandle = atoi(argv[i + 1]) ? true : false;
		}
		else if (arg == 'm')									// InjectionMethod [string]
		{
			injectionMethod = std::string(argv[i + 1]);
		}
		else if (arg == 'r')									// InjectionResources [csv list]
		{
			rawResourceIndex = i + 1;
		}

		i++;
	}

	if (rawResourceIndex >= 0) {
		if (ParseResourceValues(std::string(argv[rawResourceIndex])))
			isValidJob = true;

	}else if (jobType == Job_Get_Process_Handles && targetPid){
		isValidJob = true;
	}

}


/// <summary> Passes commandline string to files/handle values depending on injection type </summary>
/// <param name="rawString"> Raw commandline string </param>
/// <returns> true if valid resources values were found.. File paths are not validated</returns>
bool JOBDESCRIPTOR::ParseResourceValues(std::string rawString)
{
	int cIndex = 0, pIndex = -1; bool status = true;
	while (status)
	{
		pIndex = rawString.find_first_of(",", pIndex + 1);

		if (pIndex < 0) {
			if (cIndex == rawString.length()) break;
			pIndex = rawString.length();
			status = false;
		}

		if (cIndex != pIndex) {
			if (jobType == Job_Elevate) {
				HANDLE hValue; uint32_t desiredAccess;
				ParseHandleArguments( 
					rawString.substr((size_t)cIndex, (size_t)(pIndex - cIndex)).c_str(), 
					&hValue, &desiredAccess);

				if (hValue > 0 && desiredAccess >= 0) 
					handlesList.push_back({ hValue , desiredAccess });

			} else if (jobType == Job_Inject) {
				filesList.push_back(rawString.substr(cIndex, (size_t)pIndex - cIndex));
			}
		}

		cIndex = pIndex + 1;
	}

	if (jobType == Job_Elevate) {
		if (handlesList.size())
			return true;
	} else if (jobType == Job_Inject){
		if (filesList.size())
			return true;
	}

	return false;
}


/// <summary> Passes handle value and its new desired access </summary>
/// <param name="rawString"> Raw handle string separated by a ';' </param>
/// <param name="hValue"> _OUT_ [PHANDLE] Pointer to handle value </param>
/// <param name="desiredAccess">  _OUT_ [ACCESS_MASK] pointer to desired access value </param>
void JOBDESCRIPTOR::ParseHandleArguments(std::string rawString, PHANDLE hValue, uint32_t* desiredAccess)
{
	if (rawString.length() > 0)
	{
		int firstOfSC = rawString.find_first_of(";");

		if (firstOfSC == rawString.length() || firstOfSC < 0)
		{
			*hValue = (HANDLE)atoi(rawString.c_str());
			*desiredAccess = PROCESS_ALL_ACCESS;
		} else {
			*hValue = (HANDLE)atoi(rawString.substr(0,firstOfSC).c_str());
			*desiredAccess  = (uint32_t)atoi(rawString.substr(firstOfSC+1).c_str());
		}
	}else {
		*hValue = 0;
	}
	
}


int			JOBDESCRIPTOR::ResourceCount() { return (jobType == Job_Elevate) ? handlesList.size() : filesList.size(); }


bool		JOBDESCRIPTOR::IsValid() { return isValidJob; }
uint32_t	JOBDESCRIPTOR::Pid() {	return targetPid; }
JOBTYPE		JOBDESCRIPTOR::JobType() { return jobType; }
bool		JOBDESCRIPTOR::HijackHandle() { return hijackHandle; }
bool		JOBDESCRIPTOR::ElevateHandle() { return elevateHandle; }
bool		JOBDESCRIPTOR::UnloadDriverOnInject() { return unloadDriverOnInject; }
bool		JOBDESCRIPTOR::ObtainHandleViaDriver() { return obtainHandleViaDriver; }
std::string	JOBDESCRIPTOR::InjectionMethod() { return injectionMethod; }


std::string			JOBDESCRIPTOR::File( int index ) { return this->filesList[index];  }
HANDLE_NEW_ACCESS	JOBDESCRIPTOR::Handle(int index) { return handlesList[index]; }

