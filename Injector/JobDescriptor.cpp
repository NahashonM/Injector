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

	for (int i = 1; i < argc; i++)  {
		
		if (argv[i][0] != '-')	break;						// Wrong parameter

		char arg = tolower(argv[i][1]);
		
		if (arg == 't')											// Job type
		{
			jobType = (atoi(argv[i + 1]) == Job_Elevate) ? Job_Elevate : Job_Inject;
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

	if (rawResourceIndex >= 0)
		if (ParseResourceValues(std::string(argv[rawResourceIndex])))
			isValidJob = true;

}


/// <summary> Passes commandline string to files/handle values depending on injection type </summary>
/// <param name="rawString"> Raw commandline string </param>
/// <returns> true if valid resources values were found.. File paths are not validated</returns>
bool JOBDESCRIPTOR::ParseResourceValues(std::string rawString)
{
	if (jobType == Job_Elevate)
		injectionResources = new std::vector<HANDLE>;
	else if (jobType == Job_Inject)
		injectionResources = new std::vector<std::string>;
	else
		return false;

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
				HANDLE hValue = (HANDLE)atoll(rawString.substr(cIndex, pIndex - cIndex).c_str());
				if(hValue > 0)
					((std::vector<HANDLE>*)injectionResources)->push_back(hValue);
			} else if (jobType == Job_Inject) {
				((std::vector<std::string>*)injectionResources)->push_back(rawString.substr(cIndex, pIndex - cIndex));
			}
		}

		cIndex = pIndex + 1;
	}

	if (jobType == Job_Elevate) {
		if (((std::vector<HANDLE>*)injectionResources)->size())
			return true;
	} else if (jobType == Job_Inject){
		if (((std::vector<std::string>*)injectionResources)->size())
			return true;
	}

	return false;
}

