using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spec.Sniffer.Model.Battery
{
    public class BatteryStatus
    {
        private int _batteryCount;
        private ManagementObjectSearcher _wmiSearcher;
        public List<Battery> Batteries { get; set; }


        /// <summary>
        /// root\\WMI -> BatteryStatus -> Charging, ChargeRate, Discharging, DischargeRate
        /// root\\WMI -> BatteryStaticData -> DesignedCapacity, BatteryStaticData, UniqueID(serial)
        /// root\\WMI -> BatteryFullChargedCapacity -> FullChargedCapacity
        /// root\\CIMV2 -> Win32_Battery -> EstimatedChargeRemaining, Name, DeviceID
        /// </summary>

        public BatteryStatus()
        {
            
            _wmiSearcher = new ManagementObjectSearcher();
            Batteries =new List<Battery>();

            ReadCimv2Win32_Battery();
        }

        private void ReadCimv2Win32_Battery()
        {
            PrepareQuery(@"root\CIMV2", @"SELECT Name, DeviceID, EstimatedChargeRemaining FROM Win32_Battery");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                {
                    Batteries[_batteryCount].Name = (string)obj["Name"];
                    Batteries[_batteryCount].Id = (string)obj["DeviceID"];
                    Batteries[_batteryCount].EstimatedChargeRemaining = (uint)obj["EstimatedChargeRemaining"];

                    _batteryCount++;
                }
            }
            catch (Exception)
            {
                //ignore
            }

        }

        private void ReadWmiBatteryStatus()
        {

        }

        private void ReadWmiBatteryStaticData()
        {

        }

        private void ReadWmiBatteryFullChargedCapacity()
        {

        }


        private void PrepareQuery(string newScope,string newQuery)
        {

            _batteryCount = 0;

            //set scope if new
            if (_wmiSearcher.Scope.Path.NamespacePath != newScope)
                _wmiSearcher.Scope=new ManagementScope(newScope);

            //set query if new
            if (_wmiSearcher.Query.QueryString != newQuery)
                _wmiSearcher.Query = new ObjectQuery(newQuery);

        }
    }
}
