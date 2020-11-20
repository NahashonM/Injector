#pragma once


#include <string>

#ifndef _KERNEL_OBJECT_TYPES_LOCAL_DEF_
#define	_KERNEL_OBJECT_TYPES_LOCAL_DEF_

std::string objectTypes[] = {
								"DebugObject",
								"DirectoryObject",
								"Event",
								"EventPair",
								"File",
								"IoCompletion",
								"Key",
								"KeyedEvent",
								"Mutant",
								"Port",
								"Process",
								"Profile",
								"Section",
								"Semaphore",
								"SymbolicLink",
								"Thread",
								"Timer",
								"Token"
							};

#endif	//_KERNEL_OBJECT_TYPES_LOCAL_DEF_

typedef enum ObjectTypeIndex {
	DebugObject,
	DirectoryObject,
	Event,
	EventPair,
	File,
	IoCompletion,
	Key,
	KeyedEvent,
	Mutant,
	Port,
	Process,
	Profile,
	Section,
	Semaphore,
	SymbolicLink,
	Thread,
	Timer,
	Token
};