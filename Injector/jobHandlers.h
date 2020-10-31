#pragma once

#include "Util.h"
#include "JobDescriptor.h"
#include "kprocesshacker.h"
#include "cpuzdrvr.h"

#include <iostream>

/// <summary> Enumerate a processes handles </summary>
/// <param name="job"> Pointer to a job descriptor object </param>
/// <returns></returns>
bool GetProcessHandles(JOBDESCRIPTOR* job);


/// <summary> Elevate a processes handles </summary>
/// <param name="job"> Pointer to a job descriptor object </param>
/// <returns></returns>
bool ElevateProcessHandles(JOBDESCRIPTOR* job);