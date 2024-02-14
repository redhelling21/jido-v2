using Jido.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Services
{
    public interface IServiceWithStatus
    {
        public ServiceStatus Status { get; }

        public event EventHandler<ServiceStatus> StatusChanged;
    }
}
