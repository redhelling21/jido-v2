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

        public event EventHandler<EventArgs> PressEvent;

        public event EventHandler<EventArgs> ReleaseEvent;

        public AutolootService(IKeyHooksManager keyHooksManager)
        {
            _keyHooksManager = keyHooksManager;
            _keyHooksManager.RegisterKey(SharpHook.Native.KeyCode.VcZ, ZKeyPressed, ZKeyReleased);
        }

        private void ZKeyPressed(object? sender, EventArgs e)
        {
            PressEvent.Invoke(sender, e);
        }

        private void ZKeyReleased(object? sender, EventArgs e)
        {
            ReleaseEvent.Invoke(sender, e);
        }
    }

    public interface IAutolootService
    {
        public event EventHandler<EventArgs> PressEvent;

        public event EventHandler<EventArgs> ReleaseEvent;
    }
}
