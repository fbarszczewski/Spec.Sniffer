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
        public string Health
        {
            get
            {
                if (FullChargedCapacity != 0 && DesignedCapacity != 0)
                {
                    UInt32 health = DesignedCapacity * 100 / FullChargedCapacity;

                    return health < 100 ? $"{health}%" : "100%";
                }
                else
                {
                    return "0%";
                }
            }
        }

        /// <summary>
        /// Current capacity remaining in %.
        /// </summary>
        public string PowerRemaining => EstimatedChargeRemaining < 100 ? $"{EstimatedChargeRemaining}%" : "100%";

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

        /// <summary>
        /// Current battery full capacity. Used for calculation battery health.
        /// </summary>
        public UInt32 FullChargedCapacity { private get; set; }

        /// <summary>
        /// Current capacity remaining in %.Raw
        /// </summary>
        public UInt16 EstimatedChargeRemaining { private get; set; }

        public Battery()
        {
            Name = "No Battery";
            UniqueId = "";
            EstimatedChargeRemaining = 0;
            IsCharging = false;
            ChargeRate = 0;
            DesignedCapacity = 0;
            FullChargedCapacity = 0;
        }

    }
}
