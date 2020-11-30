#pragma once

#include <Windows.h>
#include <winternl.h>
#include <ntstatus.h>

namespace pcm
{

	typedef enum _INTERFACE_TYPE {
		InterfaceTypeUndefined = -1,	/* -1 */	Internal,				/* 0 */
		Isa,							/* 1 */		Eisa,					/* 2 */
		MicroChannel,					/* 3 */ 	TurboChannel,			/* 4 */
		PCIBus,							/* 5 */		VMEBus,					/* 6 */
		NuBus,							/* 7 */		PCMCIABus,				/* 8 */
		CBus,							/* 9 */		MPIBus,					/* 10 */
		MPSABus,						/* 11 */	ProcessorInternal,		/* 12 */
		InternalPowerBus,				/* 13 */	PNPISABus,				/* 14 */
		PNPBus,							/* 15 */	Vmcs,					/* 16 */
		ACPIBus,						/* 17 */	MaximumInterfaceType	/* 18 */
	} INTERFACE_TYPE, * PINTERFACE_TYPE;


	typedef enum _CM_SHARE_DISPOSITION {
		CmResourceShareUndetermined = 0,    // Reserved
		CmResourceShareDeviceExclusive,
		CmResourceShareDriverExclusive,
		CmResourceShareShared
	} CM_SHARE_DISPOSITION;


	typedef enum _RESOURCE_TYPE
	{
		CmResourceTypeNull = 0,				/* 0 */		CmResourceTypePort,				/* 1 */
		CmResourceTypeInterrupt,			/* 2 */		CmResourceTypeMemory,			/* 3 */
		CmResourceTypeDma,					/* 4 */		CmResourceTypeDeviceSpecific,	/* 5 */
		CmResourceTypeBusNumber,			/* 6 */		CmResourceTypeMemoryLarge,		/* 7 */
		CmResourceTypeNonArbitrated = 128,	/* 128 */	CmResourceTypeConfigData = 128,	/* 128 */
		CmResourceTypeDevicePrivate,		/* 129 */	CmResourceTypePcCardConfig,		/*130 */
		CmResourceTypeMfCardConfig			/* 131 */
	}RESOURCE_TYPE;

/*++																		CmResourceTypePort Bit Masks --*/
#define CM_RESOURCE_PORT_MEMORY(x)					(!(x & 0x0001))
#define CM_RESOURCE_PORT_IO(x)						(x & 0x0001)
#define CM_RESOURCE_PORT_10_BIT_DECODE(x)			(x & 0x0004)
#define CM_RESOURCE_PORT_12_BIT_DECODE(x)			(x & 0x0008)
#define CM_RESOURCE_PORT_16_BIT_DECODE(x)			(x & 0x0010)
#define CM_RESOURCE_PORT_POSITIVE_DECODE(x)			(x & 0x0020)
#define CM_RESOURCE_PORT_PASSIVE_DECODE(x)			(x & 0x0040)
#define CM_RESOURCE_PORT_WINDOW_DECODE(x)			(x & 0x0080)
#define CM_RESOURCE_PORT_BAR(x)						(x & 0x0100)

/*++																		CmResourceTypeInterrupt Bit Masks --*/
#define CM_RESOURCE_INTERRUPT_LEVEL_SENSITIVE(x)	(!(x & 0x0001))	// 0
#define CM_RESOURCE_INTERRUPT_LATCHED(x)			(x & 0x0001)	// 1
#define CM_RESOURCE_INTERRUPT_MESSAGE(x)			(x & 0x0002)	// 2
#define CM_RESOURCE_INTERRUPT_POLICY_INCLUDED(x)	(x & 0x0004)	// 4

/*++																		CmResourceTypeMemory & CmResourceTypeMemoryLarge Bit Masks --*/
#define CM_RESOURCE_MEMORY_READ_WRITE(x)			(!(x & 0x0001))
#define CM_RESOURCE_MEMORY_READ_ONLY(x)				(x & 0x0001)
#define CM_RESOURCE_MEMORY_WRITE_ONLY(x)			(x & 0x0002)
#define CM_RESOURCE_MEMORY_WRITEABILITY_MASK(x)		(x & 0x0003)
#define CM_RESOURCE_MEMORY_PREFETCHABLE(x)			(x & 0x0004)

#define CM_RESOURCE_MEMORY_COMBINEDWRITE(x)			(x & 0x0008)
#define CM_RESOURCE_MEMORY_24(x)					(x & 0x0010)
#define CM_RESOURCE_MEMORY_CACHEABLE(x)				(x & 0x0020)
#define CM_RESOURCE_MEMORY_WINDOW_DECODE(x)			(x & 0x0040)
#define CM_RESOURCE_MEMORY_BAR(x)					(x & 0x0080)

/*++																		CmResourceTypeMemoryLarge Bit Masks --*/
#define CM_RESOURCE_MEMORY_LARGE(x)					(x & 0x0E00)
#define CM_RESOURCE_MEMORY_LARGE_40(x)				(x & 0x0200)
#define CM_RESOURCE_MEMORY_LARGE_48(x)				(x & 0x0400)
#define CM_RESOURCE_MEMORY_LARGE_64(x)				(x & 0x0800)

/*++																		CmResourceTypeDma Bit Masks --*/
#define CM_RESOURCE_DMA_8(x)						(!(x & 0x0001))
#define CM_RESOURCE_DMA_16(x)						(x & 0x0001)
#define CM_RESOURCE_DMA_32(x)						(x & 0x0002)
#define CM_RESOURCE_DMA_8_AND_16(x)					(x & 0x0004)
#define CM_RESOURCE_DMA_BUS_MASTER(x)				(x & 0x0008)
#define CM_RESOURCE_DMA_TYPE_A(x)					(x & 0x0010)
#define CM_RESOURCE_DMA_TYPE_B(x)					(x & 0x0020)
#define CM_RESOURCE_DMA_TYPE_F(x)					(x & 0x0040)



	typedef LARGE_INTEGER PHYSICAL_ADDRESS;


#pragma pack(push)
#pragma pack(1)


	typedef struct _CM_PARTIAL_RESOURCE_DESCRIPTOR {
		UCHAR  Type;
		UCHAR  ShareDisposition;
		USHORT Flags;
		union {

			struct {
				PHYSICAL_ADDRESS Start;
				ULONG            Length;
			} Generic;				/* **** 12 **** */

			struct {					/*	CmResourceTypePort	*/
				PHYSICAL_ADDRESS Start;
				ULONG            Length;
			} Port;				/* **** 12 **** */

			struct {					/*	CmResourceTypeInterrupt	*/
				ULONG     Level;
				ULONG     Vector;
				KAFFINITY Affinity;
			} Interrupt;		/* **** 16 **** */

			struct {					/*	CmResourceTypeInterrupt	*/

				union {
					struct {
						USHORT    Group;
						//USHORT    Reserved;
						USHORT    MessageCount;
						ULONG     Vector;
						KAFFINITY Affinity;
					} Raw;	/* 18 */

					struct {
						ULONG     Level;
						ULONG     Vector;
						KAFFINITY Affinity;
					} Translated;/* 16 */
				} DUMMYUNIONNAME;

			} MessageInterrupt;		/* **** 18 **** */

			struct {					/*	CmResourceTypeMemory	*/
				PHYSICAL_ADDRESS Start;
				ULONG            Length;
			} Memory;				/* **** 12 **** */

			struct {					/*	CmResourceTypeDma !(V3)	*/
				ULONG Channel;
				ULONG Port;
				ULONG Reserved1;
			} Dma;					/* **** 12 **** */

			struct {					/*	CmResourceTypeDma (V3)	*/
				ULONG Channel;
				ULONG RequestLine;
				UCHAR TransferWidth;
				UCHAR Reserved1;
				UCHAR Reserved2;
				UCHAR Reserved3;
			} DmaV3;				/* **** 12 **** */

			struct {					/*	CmResourceTypeDevicePrivate | CmResourceTypePcCardConfig | CmResourceTypeMfCardConfig */
				ULONG Data[3];
			} DevicePrivate;		/* **** 12 **** */

			struct {					/*	CmResourceTypeBusNumber */
				ULONG Start;
				ULONG Length;
				ULONG Reserved;
			} BusNumber;			/* **** 12 **** */

			struct {					/* CmResourceTypeDeviceSpecific */
				ULONG DataSize;
				ULONG Reserved1;
				ULONG Reserved2;
			} DeviceSpecificData;	/* **** 12 **** */

			struct {					/*	CmResourceTypeMemoryLarge (40)	*/
				PHYSICAL_ADDRESS Start;
				ULONG            Length40;
			} Memory40;				/* **** 12 **** */

			struct {					/*	CmResourceTypeMemoryLarge (48)	*/
				PHYSICAL_ADDRESS Start;
				ULONG            Length48;
			} Memory48;				/* **** 12 **** */

			struct {					/*	CmResourceTypeMemoryLarge (64)	*/
				PHYSICAL_ADDRESS Start;
				ULONG            Length64;
			} Memory64;				/* **** 12 **** */

			struct {					/* CmResourceTypeConnection */
				UCHAR Class;
				UCHAR Type;
				UCHAR Reserved1;
				UCHAR Reserved2;
				ULONG IdLowPart;
				ULONG IdHighPart;
			} Connection;			/* **** 12 **** */
		} u;
	} CM_PARTIAL_RESOURCE_DESCRIPTOR, * PCM_PARTIAL_RESOURCE_DESCRIPTOR;


	typedef struct _CM_PARTIAL_RESOURCE_LIST {
		USHORT                         Version;
		USHORT                         Revision;
		ULONG                          Count;
		CM_PARTIAL_RESOURCE_DESCRIPTOR PartialDescriptors[1];
	} CM_PARTIAL_RESOURCE_LIST, * PCM_PARTIAL_RESOURCE_LIST;


	typedef struct _CM_FULL_RESOURCE_DESCRIPTOR {
		INTERFACE_TYPE           InterfaceType;	// type of bus the device is connected
		ULONG                    BusNumber;		// system-assigned, driver-supplied, zero-based number of the bus to which the device is connected
		CM_PARTIAL_RESOURCE_LIST PartialResourceList;
	} CM_FULL_RESOURCE_DESCRIPTOR, * PCM_FULL_RESOURCE_DESCRIPTOR;


	// specifies all of the system hardware resources assigned to a device
	typedef struct _CM_RESOURCE_LIST {
		ULONG                       Count;		// number of full resource descriptors but drivers = 1
		CM_FULL_RESOURCE_DESCRIPTOR	List[1];	// header for first full desc
	} CM_RESOURCE_LIST, * PCM_RESOURCE_LIST;



};