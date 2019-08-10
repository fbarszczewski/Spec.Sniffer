using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spec.Sniffer.Model.Battery
{
    public class Battery
    {
        public int Health { get; set; }
        public int PowerLeft { get; set; }
        public bool? IsCharging { get; set; }
        public int ChargeRate { get; set; }
        public string Serial { get; set; }

    }
}
