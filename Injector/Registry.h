#pragma once

#include <string>
#include <ntstatus.h>

#include "Util.h"
#include "ntdefs.h"


namespace reg
{

	typedef enum _KEY_INFORMATION_CLASS {
		KeyBasicInformation, KeyNodeInformation, KeyFullInformation, KeyNameInformation
	} KEY_INFORMATION_CLASS;


	typedef enum _KEY_VALUE_INFORMATION_CLASS
	{
		KeyValueBasicInformation, KeyValueFullInformation,
		KeyValuePartialInformation, KeyValueFullInformationAlign64
	} KEY_VALUE_INFORMATION_CLASS;



	typedef struct _KEY_BASIC_INFORMATION {								// KEY_INFORMATION_CLASS 0
		LARGE_INTEGER LastWriteTime;
		ULONG TitleIndex;
		ULONG NameLength;
		WCHAR Name[1];            // Variable length string
	} KEY_BASIC_INFORMATION, * PKEY_BASIC_INFORMATION;


	typedef struct _KEY_FULL_INFORMATION {								// KEY_INFORMATION_CLASS 2
		LARGE_INTEGER LastWriteTime;
		ULONG TitleIndex;
		ULONG ClassOffset;
		ULONG ClassLength;
		ULONG SubKeys;
		ULONG MaxNameLen;
		ULONG MaxClassLen;
		ULONG Values;
		ULONG MaxValueNameLen;
		ULONG MaxValueDataLen;
		WCHAR Class[1];           // Variable length string
	} KEY_FULL_INFORMATION, * PKEY_FULL_INFORMATION;

	typedef struct _KEY_NAME_INFORMATION {
		ULONG NameLength;
		WCHAR Name[1];            // Variable length string
	} KEY_NAME_INFORMATION, * PKEY_NAME_INFORMATION;


	typedef struct _KEY_VALUE_BASIC_INFORMATION {
		ULONG TitleIndex;
		ULONG Type;
		ULONG NameLength;
		WCHAR Name[1];            // Variable length string
	} KEY_VALUE_BASIC_INFORMATION, * PKEY_VALUE_BASIC_INFORMATION;


	typedef struct _KEY_VALUE_FULL_INFORMATION {
		ULONG TitleIndex;
		ULONG Type;
		ULONG DataOffset;
		ULONG DataLength;
		ULONG NameLength;
		WCHAR Name[1];            // Variable length string
								  //  Data[1];            // Variable length data not declared
	} KEY_VALUE_FULL_INFORMATION, * PKEY_VALUE_FULL_INFORMATION;

	typedef struct _KEY_VALUE_PARTIAL_INFORMATION {
		ULONG TitleIndex;
		ULONG Type;
		ULONG DataLength;
		UCHAR Data[1];            // Variable length data
	} KEY_VALUE_PARTIAL_INFORMATION, * PKEY_VALUE_PARTIAL_INFORMATION;




	/// <summary> Get handle to registry key [NtOpenKey / ZwOpenKey] </summary>
	/// <param name="pKeyHandle">  Receives opened key Handle </param>
	/// <param name="DesiredAccess"> Access required to key </param>
	/// <param name="ObjectAttributes"> Ptr to a structure that specifies the object’s attributes </param>
	/// <returns> Handle to the key</returns>
	typedef NTSTATUS(NTAPI* _NtOpenKey)
		(PHANDLE pKeyHandle, ACCESS_MASK DesiredAccess, POBJECT_ATTRIBUTES ObjectAttributes);


	/// <summary> query infor about a key object. via NtQueryKey </summary>
	/// <param name="KeyHandle"> ? </param>
	/// <param name="KeyInformationClass"> ? </param>
	/// <param name="KeyInformation"> ? </param>
	/// <param name="KeyInformationLength"> ? </param>
	/// <param name="ResultLength"> ? </param>
	/// <returns> Handle to process</returns>
	typedef NTSTATUS(NTAPI* _NtQueryKey)
		(HANDLE KeyHandle, KEY_INFORMATION_CLASS KeyInformationClass,
			PVOID KeyInformation, ULONG KeyInformationLength, PULONG ResultLength);


	/// <summary> query infor about a key object. via NtQueryValueKey </summary>
	/// <param name="KeyHandle"> ? </param>
	/// <param name="ValueName"> ? </param>
	/// <param name="KeyValueInformationClass"> ? </param>
	/// <param name="KeyValueInformation"> ? </param>
	/// <param name="KeyValueInformationLength"> ? </param>
	/// <param name="ResultLength"> ? </param>
	/// <returns> Handle to process</returns>
	typedef NTSTATUS(NTAPI* _NtQueryValueKey)
		(HANDLE KeyHandle, PUNICODE_STRING ValueName,
			KEY_VALUE_INFORMATION_CLASS KeyValueInformationClass, PVOID KeyValueInformation,
			ULONG KeyValueInformationLength, OUT PULONG ResultLength);

	static class Key
	{
	private:
		/// <summary> Query value of a registry key value </summary>
		/// <param name="hKey"> ? </param>
		/// <param name="valueName"> ? </param>
		/// <param name="infoClass"> ? </param>
		/// <param name="keyInfoBuffer"> ? </param>
		/// <param name="inforSize"> ? </param>
		/// <returns> bool []</returns>
		static bool	QueryNameValue(HANDLE hKey, std::wstring valueName, reg::KEY_VALUE_INFORMATION_CLASS infoClass, PVOID* keyInfoBuffer, PSIZE_T inforSize);

	public:
		
		/// <summary> Query value of a registry key </summary>
		/// <param name="registryKey"> ? </param>
		/// <param name="ValueName"> ? </param>
		/// <param name="destinationBuffer"> ? </param>
		/// <param name="infoClass"> ? </param>
		/// <returns> bool []</returns>
		static bool QueryRegistryKeyValue(LPCWSTR registryKey, LPCWSTR valueName,
			PVOID* destinationBuffer, KEY_VALUE_INFORMATION_CLASS infoClass = KeyValuePartialInformation);
	};

}







