using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Utils
{
    [Flags]
    public enum ServiceStatus : ushort
    {
        NONE = 0,
        RUNNING = 1,
        STOPPED = 2,
        WORKING = 4,
        IDLE = 8,
        ERROR = 16
    }
}
