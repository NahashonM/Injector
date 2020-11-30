#pragma once

#include <Windows.h>
#include <string>
#include <list>

#define LIB_EXPORT extern "C" __declspec(dllexport)


#define	_STATUS_OKAY_						1
#define _CANNOT_GET_HANDLE_TO_PROCESS_		11
#define _CANNOT_QUERY_SYSTEM_HANDLES_		12
#define	_CANNOT_QUERY_SYSTEM_OBJECT_TYPES_	13


typedef const void(_stdcall* const ERROR_OCCURED_CALLBACK)(uint16_t status);

typedef const void	(_stdcall* const HANDLE_FOUND_CALLBACK)\
					(HANDLE hValue, uint32_t accessMask, const wchar_t type[], const wchar_t name[]);




LIB_EXPORT int _stdcall GetProcessHandles(	int procPid, 
											HANDLE_FOUND_CALLBACK fnHandleFound, 
											ERROR_OCCURED_CALLBACK fnErrorOccured
										);



LIB_EXPORT int _stdcall ElevateProcessHandles(	int targetPid,
												int handleCount,
												HANDLE handles[], 
												DWORD newAccessMasks[],
												ERROR_OCCURED_CALLBACK fnErrorOccured
											);



typedef const void (_stdcall* const TestF)(uint64_t);
LIB_EXPORT bool _stdcall InjectLibraries(int count, HANDLE handles[], TestF fnTest);