#pragma once

#include <vector>
#include <algorithm>

#include "Util.h"
#include "Driver.h"
#include "Registry.h"
#include "PhysicalMemoryRes.h"

#include "kph_types.h"
#include "kph_types.h"


#define KPH_FILE_NAME		L"kprocesshacker.sys"
#define KPH_SERVICE_NAME	L"kprocesshacker2"
#define KPH_DEVICE_NAME		L"\\Device\\kprocesshacker2"
#define KPH_DEVICE_TYPE		(0x9999)



#define SYSTEM_PID					4
#define	KERNEL_MODULE_NAME			"ntoskrnl.exe"
#define PHYSICAL_MEM_HANDLE_NAME	L"\\Device\\PhysicalMemory"




class KProcessHacker : public Driver
{
private:
	bool driverImagePresent;


	HANDLE		hPhysicalMemory;
	MEMORY_MAP	memoryMap;



	/// <summary> Obtain an handle to physical memory </summary>
	/// <returns> bool: [true: success]</returns>
	bool GetHandleToPhysicalMemory();



	/// <summary> Finds a handles to system objects </summary>
	/// <param name="handleName"> Name of the object [ignored if empty string]</param>
	/// <param name="OwnerPID"> PID of the handle owner [ignored if less than -1  [-1 id current process]] </param>
	/// <param name="handleValue"> Handle Value [ignored if less than 0 ]</param>
	/// <param name="dwDesiredAccess"> Desired permissions [ignored if -1]</param>
	/// <returns> HANDLE_LIST </returns>
	HANDLE_LIST	FindHandleByName(std::wstring handleName, int OwnerPID, DWORD desiredAccess);



	/// <summary> Duplicate an handle to an object via driver </summary>
	/// <param name="hHandleToDup"> handle to be duplicated </param>
	/// <param name="hSrcProc"> handle to owning process </param>
	/// <param name="hDstProc"> handle to process receivig new handle </param>
	/// <param name="desiredAccess"> Access mask for new handle </param>
	/// <param name="hAttributes"> _opt_ attributes for new handle </param>
	/// <param name="hOptions"> DUPLICATE_CLOSE_SOURCE or NULL only </param>
	/// <returns> HANDLE </returns>
	HANDLE KPHDuplicateObjectHandle(HANDLE hHandleToDup, HANDLE hSrcProc, HANDLE hDstProc, DWORD desiredAccess, DWORD hAttributes = 0, DWORD hOptions = 0);


	/// <summary> Map physical memory view to a processes virtual adress space </summary>
	/// <param name="hProcess"> handle to process physical memory is being mapped to </param>
	/// <param name="physicalAddress"> Base Physical address to start mapping </param>
	/// <param name="sizeToMap"> Memory size to be mapped </param>
	/// <param name="mappedVirtualAddress"> Virtual address to map to physical memory </param>
	/// <param name="protection"> Permissions of the new mapped section </param>
	/// <returns> bool </returns>
	bool NtMapPhysicalMemory(HANDLE hProcess, void* physicalAddress,
		PSIZE_T sizeToMap, PVOID* mappedVirtualAddress, ULONG protection = PAGE_READWRITE);


	/// <summary> Read or write to a physical memory address </summary>
	/// <param name="physicalAddress"> _IN_ The physical address </param>
	/// <param name="size"> _IN_ Size to read/write </param>
	/// <param name="inOutBuf"> _IN_ _OUT_ Buffer </param>
	/// <param name="read"> true: read   false: write </param>
	/// <returns> bool </returns>
	bool KPHReadWritePhysicalAddress(void* physicalAddress, size_t size, void* inOutBuf, bool read = true);

public:
	KProcessHacker();

	/// <summary> Load the driver process and gets a handle to it. </summary>
	/// <returns> bool: [true:initialization success]</returns>
	bool InitDriver();


	/// <summary> Gets a handle to physical memory and checks memory mapping 
	///  to avoid bsods when reading physical memory. </summary>
	/// <returns> bool: [true:initialization success]</returns>
	bool InitPhysicalMemory();


	/// <summary> Get handle to a process via driver </summary>
	/// <param name="dwDesiredAccess"> Desired permissions </param>
	/// <param name="procPID"> target process </param>
	/// <returns> Handle to process</returns>
	HANDLE KPHOpenProcess(DWORD dwDesiredAccess, DWORD procPID);



	/// <summary> Read a physical memory address </summary>
	/// <param name="physicalAddress"> _IN_ The physical address to read</param>
	/// <param name="outBuffer"> _OUT_ Output Buffer </param>
	/// <param name="size"> _IN_ Size to read </param>
	/// <returns> bool </returns>
	bool ReadPhysicalAddr(void* physicalAddress, void* outBuffer, size_t size);


	/// <summary> Write to a physical memory address </summary>
	/// <param name="physicalAddress"> _IN_ The physical address to write to</param>
	/// <param name="inBuffer"> _IN_ Input Buffer </param>
	/// <param name="size"> _IN_ Size of data to write </param>
	/// <returns> bool </returns>
	bool WritePhysicalAddr(void* physicalAddress, void* inBuffer, size_t size);


	/// <summary> Query information about an object via driver </summary>
	/// <param name="hProc"> handle to process requesting the information </param>
	/// <param name="handleValue"> Value of Handle whose infor is needed</param>
	/// <param name="kphObjInforClass"> type of information </param>
	/// <param name="buffer"> Information output </param>
	/// <param name="returnLength"> length of result </param>
	/// <returns> bool </returns>
	bool KPHQueryObjectInfor(HANDLE hProc, WORD handleValue, KPH_OBJECT_INFORMATION_CLASS kphObjInforClass, void** buffer, LPDWORD returnLength);



	~KProcessHacker();
};
