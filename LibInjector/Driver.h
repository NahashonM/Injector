#pragma once

#include <Windows.h>
#include <winternl.h>
#include <ntstatus.h>
#include <string>

#include <exception>
#include <iostream>

#pragma comment (lib, "ntdll.lib")

#define DRIVER_DISABLE_UNLOAD	0x1		// Disable the service
#define DRIVER_DELETE_SERVICE	0x2		// Deletes the service
#define DRIVER_DELETE_IMAGE		0x4		// Deletes the image


class _NOT_IMPLEMENTED_EXCEPTION_ {
public:
	_NOT_IMPLEMENTED_EXCEPTION_(const char* msg) {
		std::cout << msg << std::endl;
	}
};


class Driver
{
private:
	std::wstring imagePath;
	std::wstring imageName;
	std::wstring serviceName;
	std::wstring deviceName;

	HANDLE	hDriver;
	DWORD	unloadBehaviour;


	/// <summary> Gets a handle to the driver </summary>
	/// <returns>[true: if valid handle] [false: no valid handle]</returns>
	bool	GetDriverHandle();


public:
	/// <summary> Initializes a Driver object </summary>
	/// <param name="drvImageName"> Name of the driver image file </param>
	/// <param name="drvServiceName"> Name of the driver service </param>
	/// <param name="drvDeviceName"> Device driven by the driver :) </param>
	/// <returns>Driver object</returns>
	Driver(const std::wstring& drvFileName, const std::wstring& drvServiceName, const std::wstring& drvDeviceName);

	/// <summary> Calls the Unload Driver method </summary>
	~Driver() { UnLoadDriver(); }


	/// <summary> Starts the driver service if not arleady started </summary>
	/// <returns>[true: driver loading succedded] [false: driver loading failed]</returns>
	bool	LoadDriver();


	/// <summary> Returns the object's handle to the driver </summary>
	/// <returns>[HANDLE]</returns>
	HANDLE	GetHandle();


	/// <summary> Stops the driver service plus the unload behavious set by user </summary>
	/// <returns>[true: driver unloaded successfully]</returns>
	bool	UnLoadDriver();


	/// <summary> Read a physical memory address </summary>
	/// <param name="physicalAddress"> _IN_ The physical address to read</param>
	/// <param name="outBuffer"> _OUT_ Output Buffer </param>
	/// <param name="size"> _IN_ Size to read </param>
	/// <returns> bool </returns>
	virtual bool ReadPhysicalAddr(void* physicalAddress, void* outBuffer, size_t bufferLength) {
		throw(new _NOT_IMPLEMENTED_EXCEPTION_("Driver has no physical memory read implementation"));
	}

};