#include "Registry.h"



bool reg::Key::QueryRegistryKeyValue(LPCWSTR registryKey, LPCWSTR valueName, PVOID* destinationBuffer, KEY_VALUE_INFORMATION_CLASS infoClass)
{
	_NtOpenKey NtOpenKey = (_NtOpenKey)util::GetExportFunctionAddress(L"ntdll.dll", "NtOpenKey");
	
	UNICODE_STRING keyObjName;
	RtlInitUnicodeString(&keyObjName, registryKey);

	OBJECT_ATTRIBUTES keyObject = { sizeof(OBJECT_ATTRIBUTES), NULL, &keyObjName ,OBJ_CASE_INSENSITIVE };

	HANDLE keyHandle;
	if (!NT_SUCCESS(NtOpenKey(&keyHandle, KEY_QUERY_VALUE | KEY_ENUMERATE_SUB_KEYS, &keyObject)))
		return false;

	SIZE_T inforSize;
	return reg::Key::QueryNameValue(keyHandle, valueName, infoClass, destinationBuffer, &inforSize);
}




bool	reg::Key::QueryNameValue(HANDLE hKey, std::wstring valueName, reg::KEY_VALUE_INFORMATION_CLASS infoClass, PVOID* keyInfoBuffer, PSIZE_T inforSize)
{
	if (*keyInfoBuffer != nullptr)	return false;

	_NtQueryValueKey NtQueryValueKey = (_NtQueryValueKey)util::GetExportFunctionAddress(L"ntdll.dll", "NtQueryValueKey");

	UNICODE_STRING valName;
	RtlInitUnicodeString(&valName, valueName.c_str());

	NTSTATUS status;
	ULONG resLen = 100;
	do
	{
		void* tmpMem = realloc(*keyInfoBuffer, resLen);
		if (!tmpMem)
		{
			status = STATUS_UNSUCCESSFUL;  break;
		}

		*keyInfoBuffer = tmpMem;
		status = NtQueryValueKey(hKey, &valName, infoClass, *keyInfoBuffer, resLen, &resLen);

	} while (status == STATUS_BUFFER_TOO_SMALL || status == STATUS_INFO_LENGTH_MISMATCH
		|| status == STATUS_BUFFER_OVERFLOW);

	if (*keyInfoBuffer)
		if (NT_SUCCESS(status)) {
			if (inforSize != nullptr) * inforSize = resLen;
			return true;
		}
		else { free(*keyInfoBuffer); if (inforSize != nullptr) * inforSize = 0; }

	return false;
}
