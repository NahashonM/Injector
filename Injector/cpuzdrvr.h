#pragma once

#include "Driver.h"
#include "Util.h"

#define CPZ_FILE_NAME		L"cpuz141.sys"
#define CPZ_SERVICE_NAME	L"cpuz141"
#define CPZ_DEVICE_NAME		L"\\Device\\cpuz141"

#define LODWORD(l)			( (DWORD)( ( (DWORD_PTR)(l)) & 0xffffffff))
#define HIDWORD(l)			( (DWORD)( ( ((DWORD_PTR)(l)) >> 32) & 0xffffffff))

#define IOCTL_READ_CR		0x9C402428
#define IOCTL_READ_MEM		0x9C402420
#define IOCTL_WRITE_MEM		0x9C402430

typedef enum _CONTROL_REGISTER { CR_0, CR_2 = 2, CR_3 } CR_REGISTER;

class CpuzDrvr : public Driver
{
private:
	bool driverImagePresent;

public:
	CpuzDrvr();

	bool InitDriver();

	bool	ReadControlRegister(CR_REGISTER crRegister, PULONG64 regValue);

	void*	ProcessEprocessVirtualAddress(uint32_t processPid);


	/// <summary>Read a physical address</summary>
	/// <param name="physicalAddress">Physical address to read</param>
	/// <param name="outBuffer">Buffer to read into</param>
	/// <param name="bufferLength">Length of the output buffer [bytes]</param>
	/// <returns></returns>
	bool	ReadPhysicalAddr(void* physicalAddress, void* outBuffer, size_t bufferLength);



	~CpuzDrvr();
};

