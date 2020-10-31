#include "Util.h"

bool util::DumpBinaryImage(const std::string fileName, const unsigned char* rawData, size_t dataByteSize)
{
	if (IsFileExists(fileName)) return true;

	bool status = false;
	FILE* file;
	fopen_s(&file, fileName.c_str(), "wb");

	if ( file ) {
		
		if( fwrite(rawData, 1, dataByteSize, file) == dataByteSize)
			status = true;

		fclose(file);
	}
	
	return status;
}


bool util::IsFileExists(const std::string& fileName)
{
	struct stat buffer;
	return (stat(fileName.c_str(), &buffer) == 0);
}	


/*++																				[ ToW ]
Function:	Convert ASCII String to Wide string
--*/
std::wstring util::ToW(std::string str)
{
	wchar_t* dst = new wchar_t[(str.length() + 1) * 2];
	mbstowcs_s(NULL, dst, str.length(), str.c_str(), str.length());
	dst[(str.length() + 1) * 2] = '\0';

	return std::wstring(dst);
}


/*++																				[ ToA ]
Function:	Convert Wide String to Ascii string
--*/
std::string util::ToA(std::wstring wstr)
{
	char* dst = new char[wstr.length() + 1];
	wcstombs_s(NULL, dst, wstr.length() + 1, wstr.c_str(), wstr.length());
	dst[wstr.length() + 1] = '\0';

	return std::string(dst);
}


std::string util::NumToAscii(const LONGLONG value, int base)
{
	int bufferSize = value / 10;
	char buffer[100];
	_i64toa_s(value, buffer, 100, base);
	return std::string(buffer);
}



/// <summary> Get system information </summary>
/// <param name="inforClass"> Type of information [SYSTEM_INFORMATION_CLASS] </param>
/// <param name="buffer"> Output information </param>
/// <param name="returnLength"> byte size of the information read </param>
/// <returns> bool </returns>
bool util::GetSystemInformation(nt::SYSTEM_INFORMATION_CLASS inforClass, void** buffer, LPDWORD	returnLength)
{
	if (*buffer != nullptr)	return false;

	size_t		 bufferSize = 0;
	NTSTATUS	 status = STATUS_SUCCESS;

	do
	{
		bufferSize += 0x10000;
		void* tempMem = realloc(*buffer, bufferSize);					// Allocate memory for the buffer
		if (tempMem == nullptr) {										// avoid memory leaks
			status = STATUS_UNSUCCESSFUL;  break;
		}

		*buffer = tempMem;
		status = NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)inforClass, *buffer, bufferSize, nullptr);

	} while (status == STATUS_INFO_LENGTH_MISMATCH);


	if (*buffer)
		if (NT_SUCCESS(status)) {
			if (returnLength != nullptr) * returnLength = bufferSize;
			return true;
		}
		else { free(*buffer); if (returnLength != nullptr) * returnLength = 0; }

	return false;
}






/// <summary> Get information about a system module... [loaded drivers and kernel modules] </summary>
/// <param name="moduleInforBuffer"> Output information buffer [PSYSTEM_MODULE_INFORMATION]</param>
/// <param name="moduleName"> Name of the system module </param>
/// <returns> bool </returns>
bool util::GetSystemModuleInfor(nt::PSYSTEM_MODULE_INFORMATION moduleInforBuffer, const char* moduleName)
{
	if (moduleName == nullptr)
		return false;

	nt::PSYSTEM_MODULES pSysModules = nullptr;
	DWORD moduleCount;

	if (!util::GetSystemInformation(nt::SystemModuleInformation, (void**)& pSysModules, &moduleCount))		// Get system Modules
		return false;

	bool found = false;
	for (ULONG i = 0; i < pSysModules->NumberOfObjects; i++) {												// Look for ntoskrnl.exe

		nt::PSYSTEM_MODULE_INFORMATION sysModule = (nt::PSYSTEM_MODULE_INFORMATION) & (pSysModules->ObjectsArray[i]);

		if (_stricmp(sysModule->ImageName + sysModule->ModuleNameOffset, moduleName) == 0)
		{
			memcpy(moduleInforBuffer, sysModule, sizeof(nt::SYSTEM_MODULE_INFORMATION));
			found = true;
			break;
		}

	}

	free(pSysModules);
	return found;
}






/// <summary> Gets information about an handle </summary>
/// <param name="hHandle"> handle value </param>
/// <param name="ownerPID"> Pid of the handle owner </param>
/// <param name="hInforBuffer"> Output information buffer [hInforBuffer] </param>
/// <returns> void* </returns>
bool util::GetHandleInfor(HANDLE hHandle, DWORD ownerPID, nt::PSYSTEM_HANDLE_INFORMATION hInforBuffer)
{
	if (hHandle == INVALID_HANDLE_VALUE || !hHandle || hInforBuffer == nullptr) return false;

	nt::PSYSTEM_HANDLES pHandles = nullptr;																	// -> SEARCH 
	DWORD handleCount;
	bool retval = false;

	if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, &handleCount))			// Get system handles
		return retval;

	for (int i = 0; i < pHandles->NumberOfHandles; i++) {
		nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];
		if ((HANDLE)sysHandle->HandleValue == hHandle && sysHandle->UniqueProcessId == ownerPID) {
			memcpy(hInforBuffer, sysHandle, sizeof(nt::SYSTEM_HANDLE_INFORMATION));
			retval = true;
			break;
		}
	}
	if (handleCount)	free(pHandles);																// Clean up

	return retval;
}





/// <summary> Gets address of an exported function in a system module </summary>
/// <param name="moduleName"> Name of the module </param>
/// <param name="exportedFunction"> Name of the exported function </param>
/// <returns> void* </returns>
void* util::GetSystemModuleProcAddr(LPCSTR moduleName, LPCSTR exportFunction)
{
	// hModule = Base Address of module in our context
	HMODULE hModule = LoadLibraryA(moduleName);
	if (hModule <= 0) return nullptr;

	// Get address of exported function from module we loaded
	void* exportFunctionAddress = GetProcAddress(hModule, exportFunction);

	// Get Base Address of module in the systems context
	void* retValue = nullptr;
	nt::SYSTEM_MODULE_INFORMATION sysModule;
	if (GetSystemModuleInfor(&sysModule, moduleName))
	{
		retValue = (void*)((uint64_t)exportFunctionAddress -
			(uint64_t)hModule +
			(uint64_t)sysModule.BaseAddress
			);
	}

	FreeLibrary(hModule);
	return retValue;
}


/// <summary> Gets address of an exported function in a module </summary>
/// <param name="moduleName"> Name of the module </param>
/// <param name="exportedFunction"> Name of the exported function </param>
/// <returns> void* </returns>
void* util::GetExportFunctionAddress(LPCWSTR moduleName, LPCSTR exportedFunction)
{
	HMODULE hMod = GetModuleHandle(moduleName);
	return (hMod != 0) ? GetProcAddress(hMod, exportedFunction) : nullptr;
}


/// <summary> change a privilege </summary>
/// <param name="privilegeName"> Name of the privilege to be set </param>
/// <param name="enable"> true by default: enable </param>
/// <returns> bool </returns>
bool util::SetPrivilege(LPCWSTR privilegeName, bool enable)
{
	HANDLE hToken = nullptr;
	LUID priviledgeNameValue;
	TOKEN_PRIVILEGES tokenPrivs;

	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, &hToken))
	{
		if (hToken)	CloseHandle(hToken);
		return false;
	}

	if (!LookupPrivilegeValue(NULL, privilegeName, &priviledgeNameValue))
	{
		CloseHandle(hToken);
		return false;
	}

	tokenPrivs.PrivilegeCount = 1;
	tokenPrivs.Privileges[0].Luid = priviledgeNameValue;
	tokenPrivs.Privileges[0].Attributes = (enable) ? SE_PRIVILEGE_ENABLED : SE_PRIVILEGE_REMOVED;

	if (!AdjustTokenPrivileges(hToken, false, &tokenPrivs, 0, nullptr, nullptr))
	{
		CloseHandle(hToken);
		return false;
	}

	CloseHandle(hToken);
	return true;
}







bool util::GetPhysicalMemoryMappingInformation(MEMORY_MAP* memoryMap)
{
	reg::PKEY_VALUE_PARTIAL_INFORMATION buffer = nullptr;
	if (!reg::Key::QueryRegistryKeyValue(KEY_PHYSICAL_MEMORY_MAP, KEY_PHYSICAL_MEMORY_MAP_VALUE_NAME, (void**)& buffer, reg::KeyValuePartialInformation))
		return false;

	pcm::PCM_FULL_RESOURCE_DESCRIPTOR pcmResDesc = ((pcm::PCM_RESOURCE_LIST)buffer->Data)->List;
	pcm::PCM_PARTIAL_RESOURCE_LIST pcmPartialList = &(pcmResDesc[0].PartialResourceList);
	pcm::PCM_PARTIAL_RESOURCE_DESCRIPTOR pcmResource = pcmPartialList->PartialDescriptors;

	// Init the memory map
	//--------------------------
	if (memoryMap == nullptr) * memoryMap = {};

	memoryMap->clear();
	uint64_t memorySize = 0;

	for (int i = 0; i < pcmPartialList->Count; i++)
	{
		switch (pcmResource[i].Type)
		{
		case pcm::CmResourceTypeMemory:
			memoryMap->push_back(
				MEMORY_MAP_ENTRY{ pcmResource[i].u.Memory.Start , pcmResource[i].u.Memory.Length }
			);
			break;

		case pcm::CmResourceTypeMemoryLarge:
			if (CM_RESOURCE_MEMORY_LARGE_40(pcmResource[i].Flags))	// max len 0x0000 00FF FFFF FF00
				memorySize = ((uint64_t)pcmResource[i].u.Memory40.Length40) << 8;
			else if (CM_RESOURCE_MEMORY_LARGE_48(pcmResource[i].Flags))	// max len  0x0000 FFFF FFFF 0000
				memorySize = ((uint64_t)pcmResource[i].u.Memory48.Length48) << 16;
			else if (CM_RESOURCE_MEMORY_LARGE_64(pcmResource[i].Flags))	// max len 0xFFFF FFFF 0000 0000
				memorySize = ((uint64_t)pcmResource[i].u.Memory64.Length64) << 32;

			memoryMap->push_back(MEMORY_MAP_ENTRY{ pcmResource[i].u.Memory.Start , memorySize });
			break;

		
		}

	}

	free(buffer);

	if (memoryMap->size()) {
		std::sort(memoryMap->begin(), memoryMap->end(),
			[](MEMORY_MAP_ENTRY & a, const MEMORY_MAP_ENTRY & b) {
				return a.RegionStart.QuadPart < b.RegionStart.QuadPart;
			}
		);

		return true;
	}
	return false;
}



void* util::ProcessEprocessVirtualAddress(uint32_t processPid)
{
	//Get handle to process
	//---------------------
	HANDLE	hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, processPid);
	void* eProcessVAddr = nullptr;

	if (hProcess > 0)
	{
		// Query handle information since it leaks eprocess address
		//-----------------------------------------------------------
		nt::SYSTEM_HANDLE_INFORMATION handleInfor;
		if (util::GetHandleInfor(hProcess, GetCurrentProcessId(), &handleInfor))
			eProcessVAddr = handleInfor.Object;

		// Close Process handle
		//----------------------
		CloseHandle(hProcess);
	}

	return eProcessVAddr;
}


/// <summary> Translate a virtual address to a physical address</summary>
/// <param name="dirBase"> the page table directory base from the eprocess or CR3 register </param>
/// <param name="virtualAddress"> The virtual address to be translated </param>
/// <param name="FnReadPhysicalAddress"> A function that can read a physical memory address </param>
/// <returns>[void*]</returns>
void* util::TranslateVirtualAddress(void* dirBase, void* virtualAddress, Driver *driverObject)
{
	if(driverObject == nullptr) return nullptr;

	try {
		USHORT pageMapLevel4 = ((uint64_t)virtualAddress >> 39) & 0x1FF;	// Page Directory Pointer  [ 39 - 47 ]	(9 bits)
		USHORT pageDirPtr = ((uint64_t)virtualAddress >> 30) & 0x1FF;		// Page Directory [ 30 - 38 ]			(9 bits)
		USHORT pageDir = ((uint64_t)virtualAddress >> 21) & 0x1FF;			// Page Table [ 21 - 29 ]				(9 bits)
		USHORT pageTable = ((uint64_t)virtualAddress >> 12) & 0x1FF;		// Page Table Entry [ 12 - 20 ]			(9 bits)
		USHORT pageOffset = (uint64_t)virtualAddress & 0xFFF;				// Page Offset [ 0 - 11 ]				(12 bits)

																//  Root structure of paging hierachy (40bits)
		dirBase = (void*)((uint64_t)dirBase & 0xFFFFFFF000);	//  Physical Address at CR3 or _KPROCESS structure of _EPROCESS
																//  Each entry is a pointer to a Page Directory Pointer Table

		// An entry in the Page Map Level 4 structure is a Page Directory Pointer Table (PDPT)
		//-------------------------------------------------------------------------------------
		void* PML4E;
		if (!driverObject->ReadPhysicalAddr(
			(void*)((uint64_t)dirBase + (pageMapLevel4 * sizeof(void*))), &PML4E, sizeof(void*)))
			return nullptr;
		if (!PML4E)	return nullptr;

		// An Entry in the PDPT points to a Page Directory(PD)
		//----------------------------------------------------
		void* PDPTE;
		if (!driverObject->ReadPhysicalAddr(
			(void*)(((uint64_t)PML4E & 0xFFFFFFFFFF000) + (pageDirPtr * sizeof(void*))), &PDPTE, sizeof(void*)))
			return nullptr;
		if (!PDPTE)	return nullptr;


		// If the PDPTE’s PS flag is 1, the PDPTE maps a 1-GByte page.
		// The final physical address is : {51-30 (PDPTE)} {29:0 (virtualAddress) }
		//----------------------------------------------------------------------------
		if (((uint64_t)PDPTE & (1 << 7)) != 0)
			return (void*)(((uint64_t)PDPTE & 0xFFFFFC0000000) + ((uint64_t)virtualAddress & 0x3FFFFFFF));


		// if PS bit is 0. The PDPTE refs a Page Directory Table
		//----------------------------------------------------------------------------
		void* PDE;
		if (!driverObject->ReadPhysicalAddr(
			(void*)(((uint64_t)PDPTE & 0xFFFFFFFFFF000) + (pageDir * sizeof(void*))), &PDE, sizeof(void*)))
			return 0;
		if (!PDE)	return 0;

		// If the PDPTE’s PS flag is 1, the PDPTE maps a 2-MByte page. 
		// physical address is : {51 - 21 (PDE)} {20:0 (virtualAddress) }.
		//---------------------------------------------------------------
		if (((uint64_t)PDE & (1 << 7)) != 0)
			return (void*)(((uint64_t)PDE & 0xFFFFFFFE00000) + ((uint64_t)virtualAddress & 0x1FFFFF));


		// if PS bit is 0. The PDE refs a Page Table
		// -----------------------------------------------
		void* PTE;
		if (!driverObject->ReadPhysicalAddr(
			(void*)(((uint64_t)PDE & 0xFFFFFFFFFF000) + (pageTable * sizeof(void*))), &PTE, sizeof(void*)))
			return 0;
		if (!PTE)	return 0;

		// the PTE maps a 4-KByte page.
		// The physical address is c: {51-12 (PTE)} {11:0 (virtualAddress) }.
		//-----------------------------------------------------------------------
		return (void*)(((uint64_t)PTE & 0xFFFFFFFFFF000) + pageOffset);

	} catch (_NOT_IMPLEMENTED_EXCEPTION_ e) {
		return nullptr;
	}
}




bool util::GetPhysicalMemoryHandle(PHANDLE pHandle)
{
	if (pHandle == nullptr) return false;

	nt::_NtOpenSection NtOpenSection = (nt::_NtOpenSection)util::GetExportFunctionAddress(L"ntdll", "NtOpenSection");
	if (NtOpenSection == nullptr) return false;

	UNICODE_STRING wsMemoryDevice;
	OBJECT_ATTRIBUTES oObjAttrs;

	*pHandle = INVALID_HANDLE_VALUE;

	RtlInitUnicodeString(&wsMemoryDevice, L"\\Device\\PhysicalMemory");
	InitializeObjectAttributes(&oObjAttrs, &wsMemoryDevice, OBJ_CASE_INSENSITIVE, NULL, NULL);

	NTSTATUS status = NtOpenSection(pHandle, READ_CONTROL, &oObjAttrs);
	if (!NT_SUCCESS(
		status
	) || *pHandle <= 0)
		return false;

	return true;
}
