#include "Util.h"

bool util::DumpBinaryImage(const std::string fileName, const unsigned char* rawData, size_t dataByteSize)
{
	if (IsFileExists(fileName)) return true;

	bool status = false;
	FILE* file;
	fopen_s(&file, fileName.c_str(), "wb");

	if ( file ) {
		
		if( fwrite(rawData, 1, dataByteSize, file) == dataByteSize)
			status = true;

		fclose(file);
	}
	
	return status;
}


bool util::IsFileExists(const std::string& fileName)
{
	struct stat buffer;
	return (stat(fileName.c_str(), &buffer) == 0);
}	


/*++																				[ ToW ]
Function:	Convert ASCII String to Wide string
--*/
std::wstring util::ToW(std::string str)
{
	wchar_t* dst = new wchar_t[(str.length() + 1) * 2];
	mbstowcs_s(NULL, dst, str.length(), str.c_str(), str.length());
	dst[(str.length() + 1) * 2] = '\0';

	return std::wstring(dst);
}


/*++																				[ ToA ]
Function:	Convert Wide String to Ascii string
--*/
std::string util::ToA(std::wstring wstr)
{
	char* dst = new char[wstr.length() + 1];
	wcstombs_s(NULL, dst, wstr.length() + 1, wstr.c_str(), wstr.length());
	dst[wstr.length() + 1] = '\0';

	return std::string(dst);
}


std::string util::NumToAscii(const LONGLONG value, int base)
{
	int bufferSize = value / 10;
	char buffer[100];
	_i64toa_s(value, buffer, 100, base);
	return std::string(buffer);
}



/// <summary> Get system information </summary>
/// <param name="inforClass"> Type of information [SYSTEM_INFORMATION_CLASS] </param>
/// <param name="buffer"> Output information </param>
/// <param name="returnLength"> byte size of the information read </param>
/// <returns> bool </returns>
bool util::GetSystemInformation(nt::SYSTEM_INFORMATION_CLASS inforClass, void** buffer, LPDWORD	returnLength)
{
	if (*buffer != nullptr)	return false;

	size_t		 bufferSize = 0;
	NTSTATUS	 status = STATUS_SUCCESS;

	do
	{
		bufferSize += 0x10000;
		void* tempMem = realloc(*buffer, bufferSize);					// Allocate memory for the buffer
		if (tempMem == nullptr) {										// avoid memory leaks
			status = STATUS_UNSUCCESSFUL;  break;
		}

		*buffer = tempMem;
		status = NtQuerySystemInformation((SYSTEM_INFORMATION_CLASS)inforClass, *buffer, bufferSize, nullptr);

	} while (status == STATUS_INFO_LENGTH_MISMATCH);


	if (*buffer)
		if (NT_SUCCESS(status)) {
			if (returnLength != nullptr) * returnLength = bufferSize;
			return true;
		}
		else { free(*buffer); if (returnLength != nullptr) * returnLength = 0; }

	return false;
}






/// <summary> Get information about a system module... [loaded drivers and kernel modules] </summary>
/// <param name="moduleInforBuffer"> Output information buffer [PSYSTEM_MODULE_INFORMATION]</param>
/// <param name="moduleName"> Name of the system module </param>
/// <returns> bool </returns>
bool util::GetSystemModuleInfor(nt::PSYSTEM_MODULE_INFORMATION moduleInforBuffer, const char* moduleName)
{
	if (moduleName == nullptr)
		return false;

	nt::PSYSTEM_MODULES pSysModules = nullptr;
	DWORD moduleCount;

	if (!util::GetSystemInformation(nt::SystemModuleInformation, (void**)& pSysModules, &moduleCount))		// Get system Modules
		return false;

	bool found = false;
	for (ULONG i = 0; i < pSysModules->NumberOfObjects; i++) {												// Look for ntoskrnl.exe

		nt::PSYSTEM_MODULE_INFORMATION sysModule = (nt::PSYSTEM_MODULE_INFORMATION) & (pSysModules->ObjectsArray[i]);

		if (_stricmp(sysModule->ImageName + sysModule->ModuleNameOffset, moduleName) == 0)
		{
			memcpy(moduleInforBuffer, sysModule, sizeof(nt::SYSTEM_MODULE_INFORMATION));
			found = true;
			break;
		}

	}

	free(pSysModules);
	return found;
}






/// <summary> Gets information about an handle </summary>
/// <param name="hHandle"> handle value </param>
/// <param name="ownerPID"> Pid of the handle owner </param>
/// <param name="hInforBuffer"> Output information buffer [hInforBuffer] </param>
/// <returns> void* </returns>
bool util::GetHandleInfor(HANDLE hHandle, DWORD ownerPID, nt::PSYSTEM_HANDLE_INFORMATION hInforBuffer)
{
	if (hHandle == INVALID_HANDLE_VALUE || !hHandle || hInforBuffer == nullptr) return false;

	nt::PSYSTEM_HANDLES pHandles = nullptr;																	// -> SEARCH 
	DWORD handleCount;
	bool retval = false;

	if (!util::GetSystemInformation(nt::SystemHandleInformation, (void**)& pHandles, &handleCount))			// Get system handles
		return retval;

	for (int i = 0; i < pHandles->NumberOfHandles; i++) {
		nt::PSYSTEM_HANDLE_INFORMATION sysHandle = &pHandles->Handles[i];
		if ((HANDLE)sysHandle->HandleValue == hHandle && sysHandle->UniqueProcessId == ownerPID) {
			memcpy(hInforBuffer, sysHandle, sizeof(nt::SYSTEM_HANDLE_INFORMATION));
			retval = true;
			break;
		}
	}
	if (handleCount)	free(pHandles);																// Clean up

	return retval;
}





/// <summary> Gets address of an exported function in a system module </summary>
/// <param name="moduleName"> Name of the module </param>
/// <param name="exportedFunction"> Name of the exported function </param>
/// <returns> void* </returns>
void* util::GetSystemModuleProcAddr(LPCSTR moduleName, LPCSTR exportFunction)
{
	// hModule = Base Address of module in our context
	HMODULE hModule = LoadLibraryA(moduleName);
	if (hModule <= 0) return nullptr;

	// Get address of exported function from module we loaded
	void* exportFunctionAddress = GetProcAddress(hModule, exportFunction);

	// Get Base Address of module in the systems context
	void* retValue = nullptr;
	nt::SYSTEM_MODULE_INFORMATION sysModule;
	if (GetSystemModuleInfor(&sysModule, moduleName))
	{
		retValue = (void*)((uint64_t)exportFunctionAddress -
			(uint64_t)hModule +
			(uint64_t)sysModule.BaseAddress
			);
	}

	FreeLibrary(hModule);
	return retValue;
}


/// <summary> Gets address of an exported function in a module </summary>
/// <param name="moduleName"> Name of the module </param>
/// <param name="exportedFunction"> Name of the exported function </param>
/// <returns> void* </returns>
void* util::GetExportFunctionAddress(LPCWSTR moduleName, LPCSTR exportedFunction)
{
	HMODULE hMod = GetModuleHandle(moduleName);
	return (hMod != 0) ? GetProcAddress(hMod, exportedFunction) : nullptr;
}


/// <summary> change a privilege </summary>
/// <param name="privilegeName"> Name of the privilege to be set </param>
/// <param name="enable"> true by default: enable </param>
/// <returns> bool </returns>
bool util::SetPrivilege(LPCWSTR privilegeName, bool enable)
{
	HANDLE hToken = nullptr;
	LUID priviledgeNameValue;
	TOKEN_PRIVILEGES tokenPrivs;

	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, &hToken))
	{
		if (hToken)	CloseHandle(hToken);
		return false;
	}

	if (!LookupPrivilegeValue(NULL, privilegeName, &priviledgeNameValue))
	{
		CloseHandle(hToken);
		return false;
	}

	tokenPrivs.PrivilegeCount = 1;
	tokenPrivs.Privileges[0].Luid = priviledgeNameValue;
	tokenPrivs.Privileges[0].Attributes = (enable) ? SE_PRIVILEGE_ENABLED : SE_PRIVILEGE_REMOVED;

	if (!AdjustTokenPrivileges(hToken, false, &tokenPrivs, 0, nullptr, nullptr))
	{
		CloseHandle(hToken);
		return false;
	}

	CloseHandle(hToken);
	return true;
}
