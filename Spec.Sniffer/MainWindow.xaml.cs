using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Spec.Sniffer.Model.Battery;

namespace Spec.Sniffer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BatteryStatus battery=new BatteryStatus();


            foreach (var batt in battery.Batteries)
            {
                MessageBox.Show($"" +
                                $"Name: {batt.Name}\n" +
                                $"UniqueId: {batt.UniqueId}\n"+
                                $"Health: {batt.Health}\n"+
                                $"IsCharging: {batt.IsCharging}\n"+
                                $"ChargeRate: {batt.ChargeRate}\n"+
                                $"PowerRemaining: {batt.PowerRemaining}\n");
            }

        }
    }
}
