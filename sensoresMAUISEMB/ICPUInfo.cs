using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{
    public interface ICPUInfo
    {
        Task<string> GetCPUInfoAsync();
    }
}