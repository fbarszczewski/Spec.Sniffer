namespace Spec.Sniffer.Model.Battery
{
    public class Battery
    {
        public string Name { get; set; }
        public string UniqueId { get; set; }
        public int Health { get; set; }
        public int PowerLeft { get; set; }
        public bool IsCharging { get; set; }
        public int ChargeRate { get; set; }

        public uint DesignedCapacity { get; set; }
        public uint FullChargedCapacity { get; set; }
        public uint EstimatedChargeRemaining { get; set; }
    }
}
