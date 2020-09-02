#include "jobHandlers.h"



/// <summary> Enumerate a processes handles </summary>
/// <param name="processID"> Process PID </param>
/// <returns></returns>
bool GetProcessHandles(JOBDESCRIPTOR *job)
{
	// Initialize driver
	//-------------------------------
	KProcessHacker kph;
	kph.InitDriver();

	HANDLE searchHandle = kph.KPHOpenProcess(PROCESS_QUERY_INFORMATION, 4);
	std::string handleDesc;

	// Get List of all system handles
	//-------------------------------
	nt::PSYSTEM_HANDLES pHandles = nullptr;
	if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, nullptr)) return false;

	std::cout << "\n";

	// Enumerate the handles list
	//-------------------------------
	HANDLE_LIST foundHandles;
	for (int i = 0; i < pHandles->NumberOfHandles; i++) {
		nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];

		if (sysHandle->UniqueProcessId != job->Pid())
			continue;

		// [	handle value ; granted access ; name	]
		//------------------------------------------------
		//handleDesc = util::NumToAscii(sysHandle->HandleValue, 16) + ";";
		std::cout << sysHandle->HandleValue << ";" << sysHandle->GrantedAccess << ";";

		// Get name of handle object
		//-------------------------------
		nt::POBJECT_NAME_INFORMATION pObjName = nullptr; 
		if (kph.KPHQueryObjectInfor(searchHandle, sysHandle->HandleValue, KphObjectNameInformation, (void**)& pObjName, nullptr))
		{
			if (pObjName->Name.Length > 0)			// Named
			{
				std::cout << util::ToA(pObjName->Name.Buffer) << "\n";
			} else {								// Unnamed
				std::cout << "unnamed" << "\n";
			}
			free(pObjName);
		} else {						// Cannot Get Name
			std::cout << "<error>" << "\n";
		}
	}

	if (pHandles) free(pHandles);																// Clean up
	CloseHandle(searchHandle);

	return true;
}