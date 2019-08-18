namespace Spec.Sniffer.Model.Parts
{
    public class Storage
    {
        public string Model { get; set; }
        public string Serial { get; set; }

        /// <summary>
        ///     The media type of the physical disk (HDD,SSD).
        /// </summary>
        public MediaType StorageType { get; set; }

        public int Size { get; set; }

        /// <summary>
        ///     Storage SMART status.
        /// </summary>
        public bool Health { get; set; }
    }

    public enum MediaType : ushort
    {
        Unspecified,
        HDD = 3,
        SSD,
        SCM
    }
}