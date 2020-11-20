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

	HANDLE searchHandle = kph.KPHOpenProcess(PROCESS_QUERY_INFORMATION, job->Pid());
	std::string handleDesc;


	// Get List of all system handles
	//-------------------------------
	nt::PSYSTEM_HANDLES pHandles = nullptr;
	if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, nullptr)) return false;


	// Get All system object types
	//------------------------------------------
	std::string* ObjectTypesNames;
	nt::POBJECT_ALL_TYPES_INFORMATION allObjectTypes = nullptr;

	if (!util::GetObjectInfor(NULL, nt::ObjectAllTypesInformation, (PVOID*)& allObjectTypes, nullptr))
		return false;

	ObjectTypesNames = new std::string[allObjectTypes->NumberOfObjectTypes];

	void* nextType = allObjectTypes->ObjectTypeInformation;

	for (int k = 0; k < allObjectTypes->NumberOfObjectTypes; k++) {

		nt::POBJECT_TYPE_INFORMATION currentType = (nt::POBJECT_TYPE_INFORMATION)nextType;

		ObjectTypesNames[k] = util::ToA(currentType->TypeName.Buffer);

		nextType = (uint8_t*)currentType->TypeName.Buffer + currentType->TypeName.MaximumLength;

		// foward align next location to size of pointer memory
		nextType = (void*)((uint64_t)((uint8_t*)nextType + (sizeof(void*) - 1)) & (uint64_t)(~(sizeof(void*) - 1)));
	}
	free(allObjectTypes);

	// Enumerate the handles list
	//-------------------------------
	HANDLE_LIST foundHandles;
	for (int i = 0; i < pHandles->NumberOfHandles; i++) {
		nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];

		if (sysHandle->UniqueProcessId != job->Pid())
			continue;

		// [ handle value ; granted access ; type ; name	]
		//---------------------------------------------------
		//handleDesc = util::NumToAscii(sysHandle->HandleValue, 16) + ";";

		std::string typeName = ObjectTypesNames[sysHandle->ObjectTypeIndex-2];
	

		// Get name of handle object
		//-------------------------------
		nt::POBJECT_NAME_INFORMATION pObjName = nullptr; 
		if (kph.KPHQueryObjectInfor(searchHandle, sysHandle->HandleValue, KphObjectNameInformation, (void**)& pObjName, nullptr)) {
			
			if (pObjName->Name.Length > 0 ||
				_strcmpi("thread", ObjectTypesNames[sysHandle->ObjectTypeIndex - 2].c_str()) == 0) {

				std::cout << sysHandle->HandleValue << ";" << sysHandle->GrantedAccess << ";";
				std::cout << ObjectTypesNames[sysHandle->ObjectTypeIndex - 2] << ";";

				if (pObjName->Name.Length > 0)
					std::cout << util::ToA(pObjName->Name.Buffer) << "\n";
				else
					std::cout << "Thread" << "\n";
			}

			free(pObjName);
		}
	}

	if (pHandles) free(pHandles);																// Clean up
	CloseHandle(searchHandle);

	return true;
}