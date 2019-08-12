using System;
using System.Collections.ObjectModel;
using System.Management;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Spec.Sniffer.Model.Battery
{
    public class BatteryStatus
    {
        private readonly ManagementObjectSearcher _wmiSearcher;
        private int _batteryId;
        private readonly DispatcherTimer _timer;

        public BatteryStatus()
        {
            _wmiSearcher = new ManagementObjectSearcher();
            _timer = new DispatcherTimer();
            BatteryList = new ObservableCollection<Battery> {new Battery(), new Battery()};

            LoadAll();
            StartTimer();
        }

        public ObservableCollection<Battery> BatteryList { get; set; }


        /// <summary>
        /// Timer to refresh battery info.
        /// </summary>
        private void StartTimer()
        {
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
            LoadAll();
        }

        private void LoadAll()
        {
            //check if namespace win32_battery exists.
            if (ReadCimv2Win32_Battery())
            {
                //check if there is WMI info about battery. If not no battery?
                if (ReadWmiBatteryStatus())
                {
                    //preserve order of this two methods!!
                    ReadWmiBatteryFullChargedCapacity();
                    ReadWmiBatteryStaticData();
                }
            }
        }

        /// <summary>
        /// Reads Win32_Battery.
        /// </summary>
        /// <returns>
        /// False if namespace not found. Meaning device with no external battery like laptop.
        /// </returns>
        private bool ReadCimv2Win32_Battery()
        {
            PrepareQuery(@"root\CIMV2", @"SELECT Name, DeviceID, EstimatedChargeRemaining FROM Win32_Battery");
            bool hasNamespace = true;
            try
            {
                foreach (var obj in _wmiSearcher.Get())
                    if (_batteryId < 2) //reduced to 2 batteries
                    {
                        BatteryList[_batteryId].Name = (string) obj["Name"];
                        BatteryList[_batteryId].UniqueId = (string) obj["DeviceID"];
                        BatteryList[_batteryId].EstimatedChargeRemaining = (ushort) obj["EstimatedChargeRemaining"];

                        _batteryId++;
                    }

            }

            
            catch (Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    //namespace not found, no battery
                    hasNamespace = false;
                }
                else
                {
                    MessageBox.Show($"ReadCimv2Win32_Battery:\n{ex.Message}");
                }
            }

            return hasNamespace;
        }

        /// <summary>
        /// Read WMI BatteryStatus.
        /// </summary>
        /// <returns>
        /// Returns false if namespace not found. Meaning no battery in device.
        /// </returns>
        private bool ReadWmiBatteryStatus()
        {
            bool hasNamespace = true;
            PrepareQuery(@"root\WMI", @"SELECT Charging, Discharging, ChargeRate, DischargeRate FROM BatteryStatus");

            try
            {
                foreach (var obj in _wmiSearcher.Get())
                    if (_batteryId < 2)
                    {
                        switch ((bool) obj["Charging"])
                        {
                            case true:
                                BatteryList[_batteryId].IsCharging = true;
                                BatteryList[_batteryId].ChargeRate = (int) obj["ChargeRate"];
                                break;

                            case false:
                                BatteryList[_batteryId].IsCharging = false;
                                BatteryList[_batteryId].ChargeRate = -(int) obj["DischargeRate"];
                                break;
                        }

                        _batteryId++;
                    }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    //namespace not found, no battery
                    hasNamespace = false;
                }
                else
                {
                    MessageBox.Show($"ReadWmiBatteryStatus:\n{ex.Message}");
                }
            }

            return hasNamespace;
        }

        private void ReadWmiBatteryFullChargedCapacity()
        {
            PrepareQuery(@"root\WMI", @"SELECT FullChargedCapacity FROM BatteryFullChargedCapacity");

            try
            {
                if (_batteryId < 2)
                    foreach (var obj in _wmiSearcher.Get())
                    {
                        BatteryList[_batteryId].FullChargedCapacity = (uint) obj["FullChargedCapacity"];

                        _batteryId++;
                    }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    //namespace not found, no battery
                }
                else
                {
                    MessageBox.Show($"ReadWmiBatteryFullChargedCapacity:\n{ex.Message}");
                }
            }
        }

        private void ReadWmiBatteryStaticData()
        {
            PrepareQuery(@"root\WMI", @"SELECT DesignedCapacity FROM BatteryStaticData");

            try
            {
                if (_batteryId < 2)
                    foreach (var obj in _wmiSearcher.Get())
                    {
                        BatteryList[_batteryId].DesignedCapacity = (uint) obj["DesignedCapacity"];

                        _batteryId++;
                    }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    //namespace not found, no battery
                }
                else
                {
                    MessageBox.Show($"ReadWmiBatteryStaticData:\n{ex.Message}");
                }
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