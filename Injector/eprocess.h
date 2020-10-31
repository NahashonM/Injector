#pragma once

#include <Windows.h>

#define		SIZE_LIST_TO_PROCESS_NAME		0x160
#define		PROCESS_ID_OFFSET				0x2E8
#define		PROCESS_NAME_OFFSET				0x450
#define		ACTIVE_PROCESS_LIST_OFFSET		0x2F0
#define		EPROCESS_IMAGE_FILE_NAME_SIZE	15

#define		EPROCESS_PEB_OFFSET				0x3F8
#define		EPROCESS_PROCESS_FLAG			0x6FC		// SYSTEM_PROCESS Flag pos 12, 1 bit
#define		EPROCESS_OBJECT_TABLE_OFFSET	0x418		//_HANDLE_TABLE


namespace nt
{

	//0x10 bytes (sizeof)
	typedef struct _LIST_ENTRY
	{
		PVOID Flink;                                              //0x0
		PVOID Blink;                                              //0x8
	}LIST_ENTRY, * PLIST_ENTRY;


	//0x8 bytes (sizeof)
	typedef struct _EX_PUSH_LOCK
	{
		union
		{
			struct
			{
				ULONGLONG Locked : 1;                                             //0x0
				ULONGLONG Waiting : 1;                                            //0x0
				ULONGLONG Waking : 1;                                             //0x0
				ULONGLONG MultipleShared : 1;                                     //0x0
				ULONGLONG Shared : 60;                                            //0x0
			};
			ULONGLONG Value;                                                    //0x0
			VOID* Ptr;                                                          //0x0
		};
	}EX_PUSH_LOCK, * PEX_PUSH_LOCK;



	//0x18 bytes (sizeof)
	typedef struct _DISPATCHER_HEADER
	{
		union
		{
			volatile LONG Lock;                                                 //0x0
			LONG LockNV;                                                        //0x0
			struct
			{
				UCHAR Type;                                                     //0x0
				UCHAR Signalling;                                               //0x1
				UCHAR Size;                                                     //0x2
				UCHAR Reserved1;                                                //0x3
			};
			struct
			{
				UCHAR TimerType;                                                //0x0
				union
				{
					UCHAR TimerControlFlags;                                    //0x1
					struct
					{
						UCHAR Absolute : 1;                                       //0x1
						UCHAR Wake : 1;                                           //0x1
						UCHAR EncodedTolerableDelay : 6;                          //0x1
					};
				};
				UCHAR Hand;                                                     //0x2
				union
				{
					UCHAR TimerMiscFlags;                                       //0x3
					struct
					{
						UCHAR Index : 6;                                          //0x3
						UCHAR Inserted : 1;                                       //0x3
						volatile UCHAR Expired : 1;                               //0x3
					};
				};
			};
			struct
			{
				UCHAR Timer2Type;                                               //0x0
				union
				{
					UCHAR Timer2Flags;                                          //0x1
					struct
					{
						UCHAR Timer2Inserted : 1;                                 //0x1
						UCHAR Timer2Expiring : 1;                                 //0x1
						UCHAR Timer2CancelPending : 1;                            //0x1
						UCHAR Timer2SetPending : 1;                               //0x1
						UCHAR Timer2Running : 1;                                  //0x1
						UCHAR Timer2Disabled : 1;                                 //0x1
						UCHAR Timer2ReservedFlags : 2;                            //0x1
					};
				};
				UCHAR Timer2ComponentId;                                        //0x2
				UCHAR Timer2RelativeId;                                         //0x3
			};
			struct
			{
				UCHAR QueueType;                                                //0x0
				union
				{
					UCHAR QueueControlFlags;                                    //0x1
					struct
					{
						UCHAR Abandoned : 1;                                      //0x1
						UCHAR DisableIncrement : 1;                               //0x1
						UCHAR QueueReservedControlFlags : 6;                      //0x1
					};
				};
				UCHAR QueueSize;                                                //0x2
				UCHAR QueueReserved;                                            //0x3
			};
			struct
			{
				UCHAR ThreadType;                                               //0x0
				UCHAR ThreadReserved;                                           //0x1
				union
				{
					UCHAR ThreadControlFlags;                                   //0x2
					struct
					{
						UCHAR CycleProfiling : 1;                                 //0x2
						UCHAR CounterProfiling : 1;                               //0x2
						UCHAR GroupScheduling : 1;                                //0x2
						UCHAR AffinitySet : 1;                                    //0x2
						UCHAR Tagged : 1;                                         //0x2
						UCHAR EnergyProfiling : 1;                                //0x2
						UCHAR SchedulerAssist : 1;                                //0x2
						UCHAR ThreadReservedControlFlags : 1;                     //0x2
					};
				};
				union
				{
					UCHAR DebugActive;                                          //0x3
					struct
					{
						UCHAR ActiveDR7 : 1;                                      //0x3
						UCHAR Instrumented : 1;                                   //0x3
						UCHAR Minimal : 1;                                        //0x3
						UCHAR Reserved4 : 3;                                      //0x3
						UCHAR UmsScheduled : 1;                                   //0x3
						UCHAR UmsPrimary : 1;                                     //0x3
					};
				};
			};
			struct
			{
				UCHAR MutantType;                                               //0x0
				UCHAR MutantSize;                                               //0x1
				UCHAR DpcActive;                                                //0x2
				UCHAR MutantReserved;                                           //0x3
			};
		};
		LONG SignalState;                                                       //0x4
		struct _LIST_ENTRY WaitListHead;                                        //0x8

	} DISPATCHER_HEADER;


	//0x2e0 bytes (sizeof)
	struct _KPROCESS
	{
		struct _DISPATCHER_HEADER Header;                                       //0x0  (18)
		struct _LIST_ENTRY ProfileListHead;                                     //0x18  (10)
		ULONGLONG DirectoryTableBase;                                           //0x28   (8)
		BYTE pad[0x2B0];														// 0x30  (2B0)

	};


	//0x880 bytes (sizeof)
	struct _EPROCESS
	{
		struct	_KPROCESS Pcb;								// 0x0
		struct	_EX_PUSH_LOCK ProcessLock;					// 0x2e0
		void*	UniqueProcessId;							// 0x2e8
		struct	_LIST_ENTRY ActiveProcessLinks;				// 0x2f0
		//char	padd[0x160];
		//char	ImageFileName[15];							// 0x450

	};


};