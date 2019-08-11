using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace Spec.Sniffer.Model.Battery
{
    public class BatteryStatus
    {
        private int _batteryId;
        private readonly ManagementObjectSearcher _wmiSearcher;

        public BatteryStatus()
        {
            _wmiSearcher = new ManagementObjectSearcher();
            Batteries = new List<Battery> {new Battery(), new Battery()};
            LoadAll();
        }

        public List<Battery> Batteries { get; set; }

        public void LoadAll()
        {
            ReadCimv2Win32_Battery();
            ReadWmiBatteryStatus();
            ReadWmiBatteryStaticData();
            ReadWmiBatteryFullChargedCapacity();
        }

        private void ReadCimv2Win32_Battery()
        {
            PrepareQuery(@"root\CIMV2", @"SELECT Name, DeviceID, EstimatedChargeRemaining FROM Win32_Battery");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                {
                    Batteries[_batteryId].Name = (string) obj["Name"];
                    Batteries[_batteryId].UniqueId = (string) obj["DeviceID"];
                    Batteries[_batteryId].EstimatedChargeRemaining = (ushort) obj["EstimatedChargeRemaining"];

                    _batteryId++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ReadCimv2Win32_Battery:\n{ex.Message}");
            }
        }

        private void ReadWmiBatteryStatus()
        {
            PrepareQuery(@"root\WMI", @"SELECT Charging, Discharging, ChargeRate, DischargeRate FROM BatteryStatus");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                {
                    switch ((bool) obj["Charging"])
                    {
                        case true:
                            Batteries[_batteryId].IsCharging = true;
                            Batteries[_batteryId].ChargeRate = (int) obj["ChargeRate"];
                            break;

                        case false:
                            Batteries[_batteryId].IsCharging = false;
                            Batteries[_batteryId].ChargeRate = -(int) obj["DischargeRate"];
                            break;
                    }

                    _batteryId++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ReadWmiBatteryStatus:\n{ex.Message}");
            }
        }

        private void ReadWmiBatteryStaticData()
        {
            PrepareQuery(@"root\WMI", @"SELECT DesignedCapacity FROM BatteryStaticData");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                {
                    Batteries[_batteryId].DesignedCapacity = (uint) obj["DesignedCapacity"];

                    _batteryId++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ReadWmiBatteryStaticData:\n{ex.Message}");
            }
        }

        private void ReadWmiBatteryFullChargedCapacity()
        {
            PrepareQuery(@"root\WMI", @"SELECT FullChargedCapacity FROM BatteryFullChargedCapacity");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                {
                    Batteries[_batteryId].FullChargedCapacity = (uint) obj["FullChargedCapacity"];

                    _batteryId++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ReadWmiBatteryFullChargedCapacity:\n{ex.Message}");
            }
        }


        /// <summary>
        ///     Sets _wmiSearcher and reset batteryId.
        /// </summary>
        /// <param name="newScope"></param>
        /// <param name="newQuery"></param>
        private void PrepareQuery(string newScope, string newQuery)
        {
            _batteryId = 0;

            //set scope if new
            if (_wmiSearcher.Scope.Path.NamespacePath != newScope)
                _wmiSearcher.Scope = new ManagementScope(newScope);

            //set query if new
            if (_wmiSearcher.Query.QueryString != newQuery)
                _wmiSearcher.Query = new ObjectQuery(newQuery);
        }
    }
}