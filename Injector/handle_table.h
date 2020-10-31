#pragma once

#include <Windows.h>


#include <vector>
#include <map>

#include "eprocess.h"


#define		Handle_TABLE_LIST_OFFSET		0x018


namespace nt
{

	typedef struct _EXHANDLE
	{
		union
		{
			struct
			{
				UINT32	TagBits : 2;	//0x0
				UINT32	Index : 30;		//0x0
			};
			VOID* GenericHandleOverlay;	//0x0
			ULONGLONG Value;			//0x0
		};
	}EXHANDLE;				//0x8 bytes (sizeof)




	typedef struct _HANDLE_TABLE_ENTRY_INFO /* Used in _HANDLE_TABLE_ENTRY, _OB_DUPLICATE_OBJECT_STATE */
	{
		UINT32	AuditMask;				//0x0
		UINT32	MaxRelativeAccessMask;	//0x4
	}HANDLE_TABLE_ENTRY_INFO;					//0x8 bytes (sizeof)




	typedef union _HANDLE_TABLE_ENTRY /* Used in  _HANDLE_TABLE_ENTRY, _HANDLE_TABLE_FREE_LIST*/
	{
		INT64 VolatileLowValue;						//0x0
		INT64 LowValue;								//0x0

		struct
		{
			UINT64		InfoTable;					//0x0	:	Ptr to _HANDLE_TABLE_ENTRY_INFO
			union
			{
				INT64		HighValue;				//0x8
				UINT64		NextFreeHandleEntry;	//0x8	:	Ptr to _HANDLE_TABLE_ENTRY
				_EXHANDLE	LeafHandleValue;		//0x8
			};
		};

		INT64	RefCountField;					//0x0

		struct
		{
			UINT64	Unlocked : 1;				//0x0
			UINT64	RefCnt : 16;				//0x0
			UINT64	Attributes : 3;				//0x0
			UINT64	ObjectPointerBits : 44;		//0x0
			UINT32	GrantedAccessBits : 25;		//0x8
			UINT32	NoRightsUpgrade : 1;		//0x8
			UINT32	Spare1 : 6;					//0x8
		};

		UINT32 Spare2;							//0xc
	}HANDLE_TABLE_ENTRY, * PHANDLE_TABLE_ENTRY;					//0x10 bytes (sizeof)




	typedef struct _RAW_HANDLE
	{
		UINT32				PhysicalAddress;
		HANDLE_TABLE_ENTRY	HandleEntry;
	}RAW_HANDLE, * PRAW_HANDLE;




	typedef struct _HANDLE_TABLE_FREE_LIST		/* used in HANDLE_TABLE structure */
	{
		EX_PUSH_LOCK		FreeListLock;			//+ 0x000
		UINT64				FirstFreeHandleEntry;	//+ 0x008  : Ptr64 _HANDLE_TABLE_ENTRY
		UINT64				LastFreeHandleEntry;	//+ 0x010  : Ptr64 _HANDLE_TABLE_ENTRY
		INT32				HandleCount;			//+ 0x018 
		UINT32				HighWaterMark;			//+ 0x01c
	}HANDLE_TABLE_FREE_LIST, * PHANDLE_TABLE_FREE_LIST;






	typedef struct _HANDLE_TABLE		/*  used in EPROCESS STRUCTURE */
	{
		UINT32			NextHandleNeedingPool;	//+ 0x000
		INT32			ExtraInfoPages;			//+ 0x004
		UINT64			TableCode;				//+ 0x008 
		UINT64			QuotaProcess;			//+ 0x010	// _Eprocess Pointer
		LIST_ENTRY		HandleTableList;		//+ 0x018	// linked list of objects
		UINT32			UniqueProcessId;		//+ 0x028 

		union
		{
			ULONG Flags;											//0x2c
			struct
			{
				UCHAR StrictFIFO : 1;								//0x2c
				UCHAR EnableHandleExceptions : 1;					//0x2c
				UCHAR Rundown : 1;									//0x2c
				UCHAR Duplicated : 1;								//0x2c
				UCHAR RaiseUMExceptionOnInvalidHandleClose : 1;		//0x2c
			};
		};

		EX_PUSH_LOCK	HandleContentionEvent;	//+ 0x030 
		EX_PUSH_LOCK	HandleTableLock;		//+ 0x038

		union
		{
			HANDLE_TABLE_FREE_LIST	FreeLists[1];				//+ 0x040
			struct
			{
				UCHAR	ActualEntry[32];						//0x40
				UINT64	DebugInfo;								//0x60 // HANDLE_TRACE_DEBUG_INFO pointer
			};
		};
	}HANDLE_TABLE, * PHANDLE_TABLE;

};			//0x80 bytes (sizeof)
