using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Spec.Sniffer.Model
{
    public class NetworkConnection
    {
        private DispatcherTimer _timer;
        public string Internet { get; set; }
        public string Ethernet { get; set; }
        public string Wifi { get; set; }



    }
}
