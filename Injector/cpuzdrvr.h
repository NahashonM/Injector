#pragma once

#include "Driver.h"
#include "Util.h"

#define CPZ_FILE_NAME		L"cpuz141.sys"
#define CPZ_SERVICE_NAME	L"cpuz141"
#define CPZ_DEVICE_NAME		L"\\Device\\cpuz141"


class CpuzDrvr : Driver
{
private:
	bool driverImagePresent;

public:
	CpuzDrvr();

	bool Init();

	~CpuzDrvr();
};

