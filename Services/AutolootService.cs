using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jido.Components.Pages.Home;
using Jido.Utils;
using SharpHook;

namespace Jido.Services
{
    public class AutolootService : IAutolootService
    {
        private IKeyHooksManager _keyHooksManager;
        public ServiceStatus Status { get; set; } = ServiceStatus.STOPPED;

        public event EventHandler<ServiceStatus> StatusChanged;

        public AutolootService(IKeyHooksManager keyHooksManager)
        {
            _keyHooksManager = keyHooksManager;
            var toggleKey = SharpHook.Native.KeyCode.VcF3;
            _keyHooksManager.RegisterKey(toggleKey, ToggleAutoloot);
        }

        private void ToggleAutoloot(object? sender, EventArgs e)
        {
            if (Status == ServiceStatus.STOPPED)
            {
                Status = ServiceStatus.IDLE;
            }
            else
            {
                Status = ServiceStatus.STOPPED;
            }
            StatusChanged?.Invoke(this, Status);
        }
    }

    public interface IAutolootService : IServiceWithStatus
    {
    }
}
