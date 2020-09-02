#pragma once

#include <Windows.h>
#include <cstdint>


namespace nt
{

	// Type of pool from which the memory for the lookaside list is allocated.
	typedef enum _POOL_TYPE
	{
		NonPagedPool, PagedPool, NonPagedPoolMustSucceed,
		DontUseThisType, NonPagedPoolCacheAligned, PagedPoolCacheAligned,
		NonPagedPoolCacheAlignedMustS, MaxPoolTypeNonPagedPoolSession = 32, PagedPoolSession,
		NonPagedPoolMustSucceedSession, DontUseThisTypeSession, NonPagedPoolCacheAlignedSession,
		PagedPoolCacheAlignedSession, NonPagedPoolCacheAlignedMustSSession
	} POOL_TYPE;


	typedef enum _SECTION_INHERIT
	{
		ViewShare = 1,
		ViewUnmap = 2
	} SECTION_INHERIT;


	// Microsoft's representation of unicode
	typedef struct _UNICODE_STRING
	{
		USHORT	Length;
		USHORT	MaximumLength;
		PWSTR	Buffer;
	} UNICODE_STRING, * PUNICODE_STRING;


	// System Information Class 1
	typedef struct _OBJECT_NAME_INFORMATION
	{
		UNICODE_STRING Name;
	} OBJECT_NAME_INFORMATION, * POBJECT_NAME_INFORMATION;

	// System Information Class 2
	typedef struct _OBJECT_TYPE_INFORMATION	
	{
		UNICODE_STRING	TypeName;				// Name identifing the object type
		ULONG			ObjectCount;			// Total number of objects of this type in the system
		ULONG			HandleCount;			// Total number of handles to objects of this type in the system
		WCHAR			Unused1[8];				//
		ULONG			PeakObjectCount;		//
		ULONG			PeakHandleCount;		//
		WCHAR			Unused2[8];				//
		ACCESS_MASK		InvalidAttributes;		//
		GENERIC_MAPPING	GenericMapping;			//
		ACCESS_MASK		ValidAttributes;		// Valid specific access rights for this object type
		BOOLEAN			SecurityRequired;		//
		BOOLEAN			MaintainHandleCount;	//
		USHORT			MaintainHandleDatabase;	//
		POOL_TYPE		PoolType;				// paged or nonpaged
		ULONG			PagedPoolUsage;			// mount of paged pool used by objects of this type
		ULONG			NonPagedPoolUsage;		//
	} OBJECT_TYPE_INFORMATION, * POBJECT_TYPE_INFORMATION;


	// System Information Class 11
	typedef struct _SYSTEM_MODULE_INFORMATION {
		ULONG	Reserved[4];
		PVOID	BaseAddress;		// base address of the module.
		ULONG	Size;				// module size
		ULONG	Flags;				// flags describing module state
		USHORT	Index;				// index of module in array of modules
		USHORT	Unknown;			// normaly contains zero
		USHORT	LoadCount;			// number of references to the module
		USHORT	ModuleNameOffset;	// offset to the final filename component of the image name.
		CHAR	ImageName[256];		// The filepath of the module
	} SYSTEM_MODULE_INFORMATION, * PSYSTEM_MODULE_INFORMATION;



	typedef struct _SYSTEM_MODULES
	{
		ULONG						NumberOfObjects;	// total handle count in the sys
		ULONG						Reserved;			// 
		SYSTEM_MODULE_INFORMATION	ObjectsArray[1];	// An array of Objects [more space is dynamically allocated]
	} SYSTEM_MODULES, * PSYSTEM_MODULES;



	// System Information Class 16
	typedef struct _SYSTEM_HANDLE_INFORMATION
	{
		DWORD UniqueProcessId;		// 4 Owner of the PID
		BYTE ObjectTypeIndex;		// 1 num identifing the Object type refered by handle [translated using infor from ZwQueryObject]
		BYTE HandleAttributes;		// 1 Flags that specify properties of the handle
		WORD HandleValue;			// 2 The numeric value of the handle.
		PVOID Object;				// 4 Address of the kernel object to which the handle refers.
		DWORD GrantedAccess;		// 4 Access granted to the handle when created.
	} SYSTEM_HANDLE_INFORMATION, * PSYSTEM_HANDLE_INFORMATION;

	typedef struct _SYSTEM_HANDLES								// 
	{
		DWORD					   NumberOfHandles;
		DWORD					   Reserved;
		SYSTEM_HANDLE_INFORMATION  Handles[1];
	} SYSTEM_HANDLES, * PSYSTEM_HANDLES;


	// System Information Class 64
	typedef struct _SYSTEM_HANDLE_INFORMATION_EX
	{
		uint64_t	Object;
		uint64_t	UniqueProcessID;
		uint64_t	HandleValue;
		ULONG		GrantedAccess;
		USHORT		CreatorBackTraceIndex;
		USHORT		ObjectTypeIndex;
		ULONG		HandleAttributes;
		ULONG		Reserved;
	} SYSTEM_HANDLE_INFORMATION_EX, * PSYSTEM_HANDLE_INFORMATION_EX;


	// System Information Class 64
	typedef struct _SYSTEM_HANDLES_EX
	{
		uint64_t					  NumberOfHandles;
		uint64_t					  Reserved;
		SYSTEM_HANDLE_INFORMATION_EX  Handles[1];
	} SYSTEM_HANDLES_EX, * PSYSTEM_HANDLES_EX;


	// Object Information Class 4
	typedef struct _OBJECT_HANDLE_ATTRIBUTE_INFORMATION
	{
		BOOLEAN Inherit;			// Can it be inherited by child
		BOOLEAN ProtectFromClose;	// Can it be closed
	}OBJECT_HANDLE_ATTRIBUTE_INFORMATION, * POBJECT_HANDLE_ATTRIBUTE_INFORMATION;
};