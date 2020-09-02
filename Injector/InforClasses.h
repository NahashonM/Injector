#pragma once

#pragma once

#include <Windows.h>
#include <winternl.h>
#include <ntstatus.h>


namespace nt
{
	// System information
	typedef enum _SYSTEM_INFORMATION_CLASS
	{
		SystemBasicInformation,                /* 0 */	SystemProcessorInformation,            /* 1 */
		SystemPerformanceInformation,          /* 2 */	SystemTimeOfDayInformation,            /* 3 */
		SystemNotImplemented1,                 /* 4 */	SystemProcessesAndThreadsInformation,  /* 5 */
		SystemCallCounts,                      /* 6 */	SystemConfigurationInformation,        /* 7 */
		SystemProcessorTimes,                  /* 8 */	SystemGlobalFlag,                      /* 9 */
		SystemNotImplemented2,                 /* 10 */	SystemModuleInformation,               /* 11 */
		SystemLockInformation,               /* 12 */	SystemNotImplemented3,               /* 13 */
		SystemNotImplemented4,               /* 14 */	SystemNotImplemented5,               /* 15 */
		SystemHandleInformation,             /* 16 */	SystemObjectInformation,             /* 17 */
		SystemPagefileInformation,           /* 18 */	SystemInstructionEmulationCounts,    /* 19 */
		SystemInvalidInfoClass1,             /* 20 */	SystemCacheInformation,              /* 21 */
		SystemPoolTagInformation,            /* 22 */	SystemProcessorStatistics,           /* 23 */
		SystemDpcInformation,                /* 24 */	SystemNotImplemented6,               /* 25 */
		SystemLoadImage,                     /* 26 */	SystemUnloadImage,                   /* 27 */
		SystemTimeAdjustment,                /* 28 */	SystemNotImplemented7,               /* 29 */
		SystemNotImplemented8,               /* 30 */	SystemNotImplemented9,               /* 31 */
		SystemCrashDumpInformation,          /* 32 */	SystemExceptionInformation,          /* 33 */
		SystemCrashDumpStateInformation,     /* 34 */	SystemKernelDebuggerInformation,     /* 35 */
		SystemContextSwitchInformation,      /* 36 */	SystemRegistryQuotaInformation,      /* 37 */
		SystemLoadAndCallImage,              /* 38 */	SystemPrioritySeparation,            /* 39 */
		SystemNotImplemented10,              /* 40 */	SystemNotImplemented11,              /* 41 */
		SystemInvalidInfoClass2,             /* 42 */	SystemInvalidInfoClass3,             /* 43 */
		SystemTimeZoneInformation,           /* 44 */	SystemLookasideInformation,          /* 45 */
		SystemSetTimeSlipEvent,              /* 46 */	SystemCreateSession,                 /* 47 */
		SystemDeleteSession,                 /* 48 */	SystemInvalidInfoClass4,             /* 49 */
		SystemRangeStartInformation,         /* 50 */	SystemVerifierInformation,           /* 51 */
		SystemAddVerifier,                   /* 52 */	SystemSessionProcessesInformation,   /* 53 */
		SystemExtendedHandleInformation = 64 /* 64 */
	} SYSTEM_INFORMATION_CLASS;

	// System Object information
	typedef enum _OBJECT_INFORMATION_CLASS
	{
		ObjectBasicInformation, ObjectNameInformation,
		ObjectTypeInformation, ObjectAllTypesInformation,
		ObjectHandleInformation
	} OBJECT_INFORMATION_CLASS;

};