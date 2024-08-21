using Jido.Utils;
using System;

namespace Jido.Services
{
    public interface IServiceWithStatus
    {
        public ServiceStatus Status { get; }

        public event EventHandler<ServiceStatus> StatusChanged;
    }
}
