using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace sensoresMAUISEMB.Models
{
    public class CoreInfo
    {
        public required string CoreName { get; set; }
        public required string Frequency { get; set; }
        public void UpdateFrequency(string frequency)
        {
            Frequency = frequency;
        }
    }

    
}
