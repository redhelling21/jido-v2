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
        public ServiceStatus AutolootStatus { get; set; } = ServiceStatus.STOPPED;

        public event EventHandler<ServiceStatus> AutolootStatusChanged;

        public AutolootService(IKeyHooksManager keyHooksManager)
        {
            _keyHooksManager = keyHooksManager;
            var toggleKey = SharpHook.Native.KeyCode.VcF3;
            _keyHooksManager.RegisterKey(toggleKey, ToggleAutoloot);
        }

        private void ToggleAutoloot(object? sender, EventArgs e)
        {
            if ((AutolootStatus & (ServiceStatus.RUNNING | ServiceStatus.WORKING)) == 0)
            {
                AutolootStatus = ServiceStatus.RUNNING | ServiceStatus.WORKING;
            }
            else
            {
                AutolootStatus = ServiceStatus.STOPPED;
            }
            AutolootStatusChanged?.Invoke(this, AutolootStatus);
        }
    }

    public interface IAutolootService
    {
        public event EventHandler<ServiceStatus> AutolootStatusChanged;

        public ServiceStatus AutolootStatus { get; set; }
    }
}
