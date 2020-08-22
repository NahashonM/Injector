#include "Driver.h"



Driver::Driver(const std::wstring& drvImageName, const std::wstring& drvServiceName, const std::wstring& drvDeviceName)
	: imageName(drvImageName), serviceName(drvServiceName), deviceName(drvDeviceName), hDriver(INVALID_HANDLE_VALUE)
{
	this->unloadBehaviour = DRIVER_DISABLE_UNLOAD | DRIVER_DELETE_SERVICE | DRIVER_DELETE_IMAGE;

	LPWSTR currentFolder = new wchar_t[MAX_PATH];
	GetCurrentDirectoryW(MAX_PATH, currentFolder);

	imageName = std::wstring(currentFolder) + L"\\" + drvImageName;

	delete[] currentFolder;
}



bool Driver::LoadDriver()
{

	if (this->hDriver != INVALID_HANDLE_VALUE || this->GetDriverHandle())				// driver Loaded  and we got an handle to it
		return true;
	// Driver not loaded

	SC_HANDLE hSCM = OpenSCManagerW(nullptr, nullptr, SC_MANAGER_CREATE_SERVICE);				// Get a handle to service Manager
	if (!hSCM) return false;																	// cant get a handle to service manager

	SC_HANDLE hDriverSvc = OpenServiceW(hSCM, this->serviceName.c_str(), SERVICE_START);				// Assume driver service exists => try to start it
	if (!hDriverSvc)																						//-----Driver Service has not been created
	{																										//------------ Create the driver service
		hDriverSvc = CreateServiceW(hSCM, this->serviceName.c_str(), this->serviceName.c_str(),
			SERVICE_START, SERVICE_KERNEL_DRIVER, SERVICE_DEMAND_START,
			SERVICE_ERROR_IGNORE, this->imageName.c_str(), nullptr,
			nullptr, nullptr, nullptr, nullptr);
		if (!hDriverSvc)																					//------------ Cannot create the service
		{
			CloseServiceHandle(hSCM);
			return false;
		}
	}

	if (!StartServiceW(hDriverSvc, 0, nullptr))									// Start the driver Service
	{																			//--- Couldn't start service
		if (GetLastError() == ERROR_SERVICE_DISABLED)							//------ if service is disabled
		{
			CloseServiceHandle(hDriverSvc);
			hDriverSvc = OpenServiceW(hSCM, this->serviceName.c_str(),		//----------- Create handle with permissions to enable service
				SERVICE_START | SERVICE_CHANGE_CONFIG);

			if (ChangeServiceConfigW(hDriverSvc, SERVICE_NO_CHANGE,				//----------- Enable Service
				SERVICE_DEMAND_START, SERVICE_NO_CHANGE, nullptr, nullptr,
				nullptr, nullptr, nullptr, nullptr, nullptr))
			{
				if (!StartServiceW(hSCM, NULL, nullptr))						//------ Couldnt start service Again
				{
					CloseServiceHandle(hSCM);
					CloseServiceHandle(hDriverSvc);
					return false;
				}

				CloseServiceHandle(hSCM);										// Service Started -> cleanup
				CloseServiceHandle(hDriverSvc);
				return true;
			}
		}

		CloseServiceHandle(hSCM);
		CloseServiceHandle(hDriverSvc);
		return false;
	}

	CloseServiceHandle(hSCM);
	CloseServiceHandle(hDriverSvc);
	return true;

}



bool Driver::GetDriverHandle()
{
	/* Use WINAPI to get a handle to the driver */
	this->hDriver = CreateFileW(this->deviceName.c_str(), GENERIC_READ | GENERIC_WRITE,
		0, nullptr, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, nullptr);

	if (this->hDriver != INVALID_HANDLE_VALUE)						// We succeeded in getting a handle to the driver
		return true;

	/* Try Nt methods to get handle to the driver */
	NTSTATUS status;
	UNICODE_STRING deviceNameUnicode;
	OBJECT_ATTRIBUTES objectAttributes;
	IO_STATUS_BLOCK isb;

	RtlInitUnicodeString(&deviceNameUnicode, this->deviceName.c_str());

	InitializeObjectAttributes(&objectAttributes, &deviceNameUnicode,					// specifies properties of the handle
		OBJ_CASE_INSENSITIVE, NULL, NULL);

	status = NtOpenFile(&(this->hDriver), FILE_GENERIC_READ | FILE_GENERIC_WRITE,
		&objectAttributes, &isb, FILE_SHARE_READ | FILE_SHARE_WRITE,
		FILE_NON_DIRECTORY_FILE | FILE_SYNCHRONOUS_IO_NONALERT);

	if (status == STATUS_SUCCESS) {														// We got a handle to the driver
		return true;
	}

	return false;																		// probably the driver is not loaded
}



HANDLE	Driver::GetHandle() { return this->hDriver; }



bool Driver::UnLoadDriver()
{
	if (this->hDriver != INVALID_HANDLE_VALUE || this->GetDriverHandle()) {		// driver is Loaded 
		CloseHandle(this->hDriver);												// Close handle
		this->hDriver = INVALID_HANDLE_VALUE;
	}
	else
		return true;															// Driver not loaded

	DWORD permission = SERVICE_STOP;

	if (this->unloadBehaviour & DRIVER_DISABLE_UNLOAD) permission |= SERVICE_CHANGE_CONFIG;
	if(this->unloadBehaviour & DRIVER_DELETE_SERVICE) permission |= DELETE;

	SC_HANDLE hSCM = OpenSCManagerW(nullptr, nullptr, SC_MANAGER_CONNECT);				//  Get handle to service manager
	if (!hSCM)	return false;

	SC_HANDLE hService = OpenServiceW(hSCM, this->serviceName.c_str(), permission);	// Get handle to the Service
	if (!hService) { CloseServiceHandle(hSCM); return false; }

	LPSERVICE_STATUS lpStatus = 0;
	if (!ControlService(hService, SERVICE_CONTROL_STOP, lpStatus))						// if service is not running => stop it
	{
		CloseServiceHandle(hService);	CloseServiceHandle(hSCM); return false;
	}

	if (permission & DRIVER_DISABLE_UNLOAD)							// Disable Service
	{
		if (!ChangeServiceConfigW(hService, SERVICE_NO_CHANGE, SERVICE_DISABLED ,
			SERVICE_NO_CHANGE, nullptr, nullptr, nullptr, nullptr,
			nullptr, nullptr, nullptr))
		{
			CloseServiceHandle(hService); CloseServiceHandle(hSCM); return false;	// Couldnt disable service
		}		
	}

	if (permission & DRIVER_DELETE_SERVICE)							// Delete the service
	{
		if (!DeleteService(hService))
		{
			CloseServiceHandle(hService);	CloseServiceHandle(hSCM); return false;	// Couldnt delete the service
		}		
	}

	if (permission & DRIVER_DELETE_IMAGE)							// Delete the Binary Image
	{
	}

	CloseServiceHandle(hService);
	CloseServiceHandle(hSCM);

	return true;
}

