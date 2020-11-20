#pragma once

#include "NtTypes.h"

#include <winternl.h>
#include <ntstatus.h>

namespace nt
{
	/// <summary> Get System properties via undocumented NtQuerySysteInformation </summary>
	/// <param name="SystemInformationClass">  System information class </param>
	/// <param name="SystemInformation"> v </param>
	/// <param name="SystemInformationLength"> size of the container in bytes </param>
	/// <param name="ReturnLength"> Byte size written to the container or a suggestion of the required size  </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtQuerySystemInformation)
		(DWORD SystemInformationClass, PVOID SystemInformation,
			DWORD SystemInformationLength, LPDWORD ReturnLength);

	/// <summary> Duplicates a system object via undocumented NtDuplicateObject </summary>
	/// <param name="SourceProcessHandle">  Handle to process to receive the handle </param>
	/// <param name="SourceHandle"> Handle to be duplicated  </param>
	/// <param name="TargetProcessHandle"> Handle to process owning the handle </param>
	/// <param name="TargetHandle"> pointer to handle object to receive the duplicated handle </param>
	/// <param name="DesiredAccess"> Required permissions of the new handle </param>
	/// <param name="Attributes"> Flags for the new handle </param>
	/// <param name="Options"> options </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtDupObj)
		(HANDLE SourceProcessHandle, HANDLE SourceHandle, HANDLE TargetProcessHandle,
			PHANDLE TargetHandle, ACCESS_MASK DesiredAccess, DWORD Attributes, DWORD Options);

	/// <summary> Query an information about a system object via undocumented NtQueryObject </summary>
	/// <param name="ObjectHandle">  Receives opened key Handle </param>
	/// <param name="ObjectInformationClass"> Class of information needed </param>
	/// <param name="ObjectInformation"> Container to receive the requested information </param>
	/// <param name="ObjectInformationLength"> size of the container in bytes </param>
	/// <param name="ReturnLength"> Byte size written to the container or a suggestion of the required size  </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtQueryObj)
		(HANDLE ObjectHandle, nt::OBJECT_INFORMATION_CLASS ObjectInformationClass, PVOID ObjectInformation,
			DWORD ObjectInformationLength, LPDWORD ReturnLength);

	/// <summary> Change an object's information via undocumented NtSetInformationObject  </summary>
	/// <param name="ObjectHandle">  Handle to the object </param>
	/// <param name="ObjectInformationClass"> Information type </param>
	/// <param name="ObjectInformation"> Container for the new information </param>
	/// <param name="Length">Length of the information container </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtSetInforObj)							// _NtSetInformationObject
		(HANDLE ObjectHandle, nt::OBJECT_INFORMATION_CLASS ObjectInformationClass,
			PVOID ObjectInformation, ULONG Length);



	/// <summary> Map a view of a section into the virtual address space of another process via NtMapViewOfSection  </summary>
	/// <param name="SectionHandle">  Handle to the section object </param>
	/// <param name="ProcessHandle"> Handle to Process that view is being mapped to </param>
	/// <param name="BaseAddress"> ptr to variable that receives the view base address </param>
	/// <param name="ZeroBits"> ? </param>
	/// <param name="CommitSize"> ? </param>
	/// <param name="SectionOffset"> optional pointer to the offset from the beginning of the section to the view </param>
	/// <param name="ViewSize"> receives the actual size of mapped view. if 0 at time of call section is mapped from offset to end of section </param>
	/// <param name="InheritDisposition"> ? </param>
	/// <param name="AllocationType"> ? </param>
	/// <param name="Win32Protect"> ? </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtMapViewOfSection)
		(HANDLE SectionHandle, HANDLE ProcessHandle, PVOID* BaseAddress,
			ULONG_PTR ZeroBits, SIZE_T CommitSize, PLARGE_INTEGER SectionOffset, PSIZE_T ViewSize,
			SECTION_INHERIT InheritDisposition, ULONG AllocationType, ULONG Win32Protect);


	/// <summary> Open a handle for an existing section object </summary>
	/// <param name="SectionHandle">  Pointer to a HANDLE variable that receives a handle to the section object </param>
	/// <param name="DesiredAccess">  Specifies an ACCESS_MASK value that determines the requested access to the object </param>
	/// <param name="ObjectAttributes"> Pointer to an OBJECT_ATTRIBUTES structure that specifies the object name and other attributes </param>
	/// <param name="Win32Protect"> ? </param>
	/// <returns> NTSTATUS 0:STATUS_SUCCESS</returns>
	typedef NTSTATUS(NTAPI* _NtOpenSection)
		(PHANDLE SectionHandle, ACCESS_MASK DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes);
};