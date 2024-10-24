using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace sensoresMAUISEMB.Models
{
    public class CpuUsageDataPoint
    {
        public double Time { get; set; }
        public double Usage { get; set; }
    }
}
