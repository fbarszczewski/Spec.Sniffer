using System.ComponentModel;

namespace Spec.Sniffer.Model.Battery
{
    public class Battery : INotifyPropertyChanged
    {
        private string _chargeRate;
        private uint _designedCapacity;
        private ushort _estimatedChargeRemaining;
        private bool _isCharging;
        private string _name;
        private string _uniqueId;

        public Battery()
        {
            Name = "No Name";
            UniqueId = "noID";
            EstimatedChargeRemaining = 0;
            IsCharging = false;
            ChargeRate = "0";
            DesignedCapacity = 0;
            FullChargedCapacity = 0;
        }

        /// <summary>
        ///     Battery name id.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        ///     Identifies the battery. Closest to serial
        /// </summary>
        public string UniqueId
        {
            get => _uniqueId;
            set
            {
                _uniqueId = value;
                RaisePropertyChanged("UniqueId");
            }
        }

        /// <summary>
        ///     Battery capacity remaining in %.
        /// </summary>
        public string Health
        {
            get
            {
                if (FullChargedCapacity != 0 && DesignedCapacity != 0)
                {
                    var health = FullChargedCapacity * 100 / DesignedCapacity;

                    return health < 100 ? $"{health}%" : "100%";
                }

                return "0%";
            }
        }

        /// <summary>
        ///     Current capacity remaining in %.
        /// </summary>
        public string PowerRemaining => EstimatedChargeRemaining < 100
            ? $"{EstimatedChargeRemaining}%  {ChargeRate}"
            : $"100% {ChargeRate}";

        /// <summary>
        ///     Is battery charging.
        /// </summary>
        public bool IsCharging
        {
            get => _isCharging;
            set
            {
                _isCharging = value;
                RaisePropertyChanged("IsCharging");
            }
        }

        /// <summary>
        ///     Charge or discharge rate. In mWh.
        /// </summary>
        public string ChargeRate
        {
            get => _chargeRate;
            set
            {
                _chargeRate = value;
                RaisePropertyChanged("ChargeRate");
            }
        }

        /// <summary>
        ///     Battery full capacity. Used for calculation battery health.
        /// </summary>
        public uint DesignedCapacity
        {
            private get { return _designedCapacity; }
            set
            {
                _designedCapacity = value;
                RaisePropertyChanged("Health");
            }
        }

        /// <summary>
        ///     Current battery full capacity. Used for calculation battery health.
        /// </summary>
        public uint FullChargedCapacity { private get; set; }

        /// <summary>
        ///     Current capacity remaining in %.Raw
        /// </summary>
        public ushort EstimatedChargeRemaining
        {
            private get { return _estimatedChargeRemaining; }
            set
            {
                _estimatedChargeRemaining = value;
                RaisePropertyChanged("PowerRemaining");
            }
        }


        #region INotify Property handler

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}