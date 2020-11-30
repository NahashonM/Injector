
#include "exports.h"


LIB_EXPORT bool _stdcall InjectLibraries(int count, HANDLE handles[], TestF fnTest)
{
	
	for (int i = 0; i < sizeof(handles); i++) {
		fnTest((uint64_t)handles[i]);
	}

	return false;
}