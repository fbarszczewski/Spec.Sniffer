using System;

namespace Spec.Sniffer.Model.Battery
{
    public class Battery
    {
        /// <summary>
        /// Battery name id.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Identifies the battery. Closest to serial
        /// </summary>
        public string UniqueId { get; set; }
        /// <summary>
        /// Battery capacity remaining in %.
        /// </summary>
        public int Health { get; set; }
        /// <summary>
        /// Current capacity remaining in %.
        /// </summary>
        public int PowerLeft { get; set; }
        /// <summary>
        /// Is battery charging.
        /// </summary>
        public bool IsCharging { get; set; }
        /// <summary>
        /// Charge or discharge rate. In mWh.
        /// </summary>
        public int ChargeRate { get; set; }

        /// <summary>
        /// Battery full capacity. Used for calculation battery health.
        /// </summary>
        public UInt32 DesignedCapacity { private get; set; }
        public UInt32 FullChargedCapacity { private get; set; }
        public UInt16 EstimatedChargeRemaining { private get; set; }
    }
}
