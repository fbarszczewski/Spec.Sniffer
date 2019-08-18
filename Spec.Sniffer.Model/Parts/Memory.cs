namespace Spec.Sniffer.Model.Parts
{
    public class Memory
    {
        public string PartNumber { get; set; }
        public string Serial { get; set; }

        /// <summary>
        ///     Total capacity of the physical memory, in GB.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        ///     Label of the socket or circuit board that holds the memory.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///     The configured clock speed of the memory device, in MHz, or 0, if the speed is unknown.
        /// </summary>
        public uint ClockSpeed { get; set; }
    }
}