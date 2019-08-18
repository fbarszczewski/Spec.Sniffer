using System.Collections.Generic;
using Spec.Sniffer.Model.Parts;

namespace Spec.Sniffer.Model.Spec
{
    public class DeviceSpec
    {
        public Chassis ChassisType { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string PartNumber { get; set; }
        public string DeviceSerial { get; set; }
        public string MotherboardSerial { get; set; }
        public string Cpu { get; set; }
        /// <summary>
        /// Motherboards memory slots capacity.
        /// </summary>
        public string MemoryBanks { get; set; }
        /// <summary>
        /// Detail list of each memory module installed in device.
        /// </summary>
        public List<Memory> MemoryList { get; set; }
        /// <summary>
        /// Sum of all memory available on device
        /// </summary>
        public string MemoryCapacity { get; set; }
        public string OpticalDrive { get; set; }
        /// <summary>
        /// Detail list of each internal storage drive in device.
        /// </summary>
        public List<Storage> StorageList { get; set; }
        public string Diagonal { get; set; }
        public string Resolution { get; set; }
        public List<string> GpuList { get; set; }
        public string CurrentOs { get; set; }
        public string CurrentOsVer { get; set; }
        public string CurrentOsLang { get; set; }



    }


    public enum Chassis : ushort
    {
        Other = 1,
        Unknown,
        Desktop,
        LowProfileDesktop,
        PizzaBox,
        MiniTower,
        Tower,
        Portable,
        Laptop,
        Notebook,
        HandHeld,
        DockingStation,
        AiO,
        SubNotebook,
        SpaceSaving,
        LunchBox,
        MainSystemChassis,
        ExpansionChassis,
        SubChassis,
        BusExpansionChassis,
        PeripheralChassis,
        StorageChassis,
        RackMountChassis,
        SealedCasePc
    }
}