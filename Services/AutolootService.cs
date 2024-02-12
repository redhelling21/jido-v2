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

        public AutolootService(IKeyHooksManager keyHooksManager)
        {
            _keyHooksManager = keyHooksManager;
        }
    }

    public interface IAutolootService
    { }
}
