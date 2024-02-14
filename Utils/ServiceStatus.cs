using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Utils
{
    [Flags]
    public enum ServiceStatus
    {
        STOPPED,
        WORKING,
        IDLE,
        ERROR,
    }
}
