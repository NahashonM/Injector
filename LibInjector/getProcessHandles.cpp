

#include "kprocesshacker.h"

#include "exports.h"


/// <summary> Enumerate a processes handles </summary>
/// <param name="procPid"> Process PID </param>
/// <param name="callback"> Callback method each time a process is identified </param>
/// <returns></returns>

LIB_EXPORT int _stdcall GetProcessHandles(int procPid, HANDLE_FOUND_CALLBACK fnHandleFound, ERROR_OCCURED_CALLBACK fnErrorOccured)
{

	DWORD status = _STATUS_OKAY_;

	// Initialize driver and get handle to process
	//---------------------------------------------
	KProcessHacker kph;
	kph.InitDriver();

	HANDLE hProcess = kph.KPHOpenProcess(PROCESS_QUERY_INFORMATION, procPid);
	if (hProcess <= 0) status = _CANNOT_GET_HANDLE_TO_PROCESS_;

	// Get List of all system handles
	//-------------------------------
	nt::PSYSTEM_HANDLES pHandles = nullptr;
	if (status == _STATUS_OKAY_) 
		if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, nullptr))
			status = _CANNOT_QUERY_SYSTEM_HANDLES_;
	


	if (status == _STATUS_OKAY_) {

		// Get All system object types
		//------------------------------------------
		std::wstring* sysObjectNames;
		nt::POBJECT_ALL_TYPES_INFORMATION sysObjectTypes = nullptr;

		if (!util::GetObjectInfor(NULL, nt::ObjectAllTypesInformation, (PVOID*)& sysObjectTypes, nullptr))
			status = _CANNOT_QUERY_SYSTEM_OBJECT_TYPES_;

		
		if (status != _CANNOT_QUERY_SYSTEM_OBJECT_TYPES_) {

			sysObjectNames = new std::wstring[sysObjectTypes->NumberOfObjectTypes];

			// Enumerate the object types getting the corresponding names
			// ------------------------------------------------------------
			void* nextType = sysObjectTypes->ObjectTypeInformation;

			for (uint32_t k = 0; k < sysObjectTypes->NumberOfObjectTypes; k++) {

				nt::POBJECT_TYPE_INFORMATION currentType = (nt::POBJECT_TYPE_INFORMATION)nextType;

				sysObjectNames[k] = currentType->TypeName.Buffer;

				nextType = (uint8_t*)currentType->TypeName.Buffer + currentType->TypeName.MaximumLength;

				// foward align next location to size of pointer memory
				//---------------------------------------------------------
				nextType = (void*)((uint64_t)((uint8_t*)nextType + (sizeof(void*) - 1)) & (uint64_t)(~(sizeof(void*) - 1)));
			}

			free(sysObjectTypes);
		} else {
			sysObjectNames = new std::wstring[1];
			*sysObjectNames = L"err";
		}


		// Enumerate the system handles
		//-------------------------------
		for (uint32_t i = 0; i < pHandles->NumberOfHandles; i++) {

			nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];

			if (sysHandle->UniqueProcessId != procPid)
				continue;

			std::wstring typeName = (status == _CANNOT_QUERY_SYSTEM_OBJECT_TYPES_)? *sysObjectNames : sysObjectNames[sysHandle->ObjectTypeIndex - 2];
			std::wstring handleName = L"err";


			// Get name of handle object if its not a thread object
			//------------------------------------------------------
			bool isNamed = true;
			if (lstrcmpiW(L"Thread", typeName.c_str()) == 0) {
				// TODO----
				// Get name of process the thread object belongs to

				handleName = L"Proc Name";

			} else {

				// get name of handle its not a thread or we cant identify type
				//------------------------------------------------------------
				nt::POBJECT_NAME_INFORMATION pObjName = nullptr;
				if (kph.KPHQueryObjectInfor(hProcess, sysHandle->HandleValue, KphObjectNameInformation, (void**)& pObjName, nullptr)) {

					if (pObjName->Name.Length > 0)
						handleName = pObjName->Name.Buffer;
					else
						isNamed = false;

					free(pObjName);
				}
			}


			// Raise the callback
			//------------------------------
			if (fnHandleFound != nullptr && isNamed) fnHandleFound((HANDLE)sysHandle->HandleValue,
																	sysHandle->GrantedAccess,
																	typeName.c_str(),
																	handleName.c_str()
																	);
			
			status = _STATUS_OKAY_;
		}


		// clean up
		//-----------------
		free(pHandles);
		CloseHandle(hProcess);
	}

	return status;
}
