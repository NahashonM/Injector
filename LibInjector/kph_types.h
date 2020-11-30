#pragma once


#define KPH_DEVICE_TYPE				(0x9999)
#define KPH_CTL_CODE(x)				CTL_CODE(KPH_DEVICE_TYPE, 0x800 + x, METHOD_NEITHER, FILE_ANY_ACCESS)


#define KPH_OPENPROCESSTOKEN		KPH_CTL_CODE(51)
#define KPH_OPENPROCESSJOB			KPH_CTL_CODE(52)


#define KPH_TERMINATEPROCESS		KPH_CTL_CODE(55)

#define KPH_GETPROCESSPROTECTED		KPH_CTL_CODE(59)
#define KPH_SETPROCESSPROTECTED		KPH_CTL_CODE(60)
#define KPH_SETEXECUTEOPTIONS		KPH_CTL_CODE(61)


#define KPH_OPENPROCESS				KPH_CTL_CODE(50)
#define KPH_QUERYINFORMATIONOBJECT KPH_CTL_CODE(151)

#define KPH_SUSPENDPROCESS			KPH_CTL_CODE(53)
#define KPH_RESUMEPROCESS			KPH_CTL_CODE(54)

#define KPH_READVIRTUALMEMORY		KPH_CTL_CODE(56)
#define KPH_WRITEVIRTUALMEMORY		KPH_CTL_CODE(57)
#define KPH_UNSAFEREADVIRTUALMEMORY	KPH_CTL_CODE(58)

#define KPH_DUPLICATEOBJECT KPH_CTL_CODE(153)



typedef enum _KPH_OBJECT_INFORMATION_CLASS : uint64_t
{
	KphObjectBasicInformation, // q: OBJECT_BASIC_INFORMATION
	KphObjectNameInformation, // q: OBJECT_NAME_INFORMATION
	KphObjectTypeInformation, // q: OBJECT_TYPE_INFORMATION
	KphObjectHandleFlagInformation, // qs: OBJECT_HANDLE_FLAG_INFORMATION
	KphObjectProcessBasicInformation, // q: PROCESS_BASIC_INFORMATION
	KphObjectThreadBasicInformation, // q: THREAD_BASIC_INFORMATION
	KphObjectEtwRegBasicInformation, // q: ETWREG_BASIC_INFORMATION
	KphObjectFileObjectInformation, // q: KPH_FILE_OBJECT_INFORMATION
	KphObjectFileObjectDriver, // q: KPH_FILE_OBJECT_DRIVER
	MaxKphObjectInfoClass
} KPH_OBJECT_INFORMATION_CLASS;



#define MEMORY_MAP_BASE(x, _mem_map_)			_mem_map_[x].RegionStart.QuadPart
#define MEMORY_MAP_SIZE(x, _mem_map_)			_mem_map_[x].RegionSize
#define MEMORY_MAP_END(x, _mem_map_)			(_mem_map_[x].RegionStart.QuadPart + _mem_map_[x].RegionSize-1)



typedef	std::vector<HANDLE>				HANDLE_LIST;