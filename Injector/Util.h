#pragma once

#include <Windows.h>
#include <string>
#include <fstream>

#include "ntdefs.h"

namespace util
{

	/// <summary> Dumps raw binary data to file </summary>
	/// <param name="fileName"> Name of the output biary file </param>
	/// <param name="rawData"> The raw binary data </param>
	/// <param name="dataByteSize"> Size of the data in bytes </param>
	/// <returns>bool: true if successful</returns>
	bool DumpBinaryImage(const std::string fileName, const unsigned char* rawData, size_t dataByteSize);


	/// <summary> Checks if a file exists </summary>
	/// <param name="fileName"> Name of the file </param>
	/// <returns>bool: true if file exists</returns>
	bool IsFileExists(const std::string& fileName);


	/// <summary> Converts a c style string to ms wide string </summary>
	/// <param name="str"> standard c string </param>
	/// <returns> wide string </returns>
	std::wstring ToW(std::string str);

	/// <summary> Converts a wide string to a c style string </summary>
	/// <param name="wstr"> wide string </param>
	/// <returns>c style string</returns>
	std::string ToA(std::wstring wstr);


	std::string NumToAscii(const LONGLONG value, int base);



	/// <summary> Get system information </summary>
	/// <param name="inforClass"> Type of information [SYSTEM_INFORMATION_CLASS] </param>
	/// <param name="buffer"> Output information </param>
	/// <param name="returnLength"> byte size of the information read </param>
	/// <returns> bool </returns>
	bool GetSystemInformation(nt::SYSTEM_INFORMATION_CLASS inforClass, void** buffer, LPDWORD returnLength);

	/// <summary> Get information about a system module... [loaded drivers and kernel modules] </summary>
	/// <param name="moduleInforBuffer"> Output information buffer [PSYSTEM_MODULE_INFORMATION]</param>
	/// <param name="moduleName"> Name of the system module </param>
	/// <returns> bool </returns>
	bool GetSystemModuleInfor(nt::PSYSTEM_MODULE_INFORMATION moduleInforBuffer,	const char* moduleName);


	/// <summary> Gets information about an handle </summary>
	/// <param name="hHandle"> handle value </param>
	/// <param name="ownerPID"> Pid of the handle owner </param>
	/// <param name="hInforBuffer"> Output information buffer [hInforBuffer] </param>
	/// <returns> void* </returns>
	bool GetHandleInfor(HANDLE hHandle, DWORD ownerPID, nt::PSYSTEM_HANDLE_INFORMATION hInforBuffer);


	/// <summary> Gets address of an exported function in a system module </summary>
	/// <param name="moduleName"> Name of the module </param>
	/// <param name="exportedFunction"> Name of the exported function </param>
	/// <returns> void* </returns>
	void* GetSystemModuleProcAddr(LPCSTR moduleName, LPCSTR exportFunction);


	/// <summary> Gets address of an exported function in a module </summary>
	/// <param name="moduleName"> Name of the module </param>
	/// <param name="exportedFunction"> Name of the exported function </param>
	/// <returns> void* </returns>
	void* GetExportFunctionAddress(LPCWSTR moduleName, LPCSTR exportedFunction);


	/// <summary> change a privilege </summary>
	/// <param name="privilegeName"> Name of the privilege to be set </param>
	/// <param name="enable"> true by default: enable </param>
	/// <returns> bool </returns>
	bool SetPrivilege(LPCWSTR privilegeName, bool enable = true);

};