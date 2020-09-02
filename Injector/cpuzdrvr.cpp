#include "cpuzdrvr.h"

#include "cpuz141_shellcode.h"


CpuzDrvr::CpuzDrvr()
	: Driver(CPZ_FILE_NAME, CPZ_SERVICE_NAME, CPZ_DEVICE_NAME)
{
	driverImagePresent =
		util::DumpBinaryImage(util::ToA(CPZ_FILE_NAME), cpuz141ShellCode, sizeof(cpuz141ShellCode));
}


bool CpuzDrvr::Init()
{
	if (!LoadDriver()) return false;
	return true;
}



CpuzDrvr::~CpuzDrvr()
{
}
