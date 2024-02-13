using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jido.Services;

namespace Jido.Utils
{
    public class StatusManager
    {
        private IAutolootService _autolootService;

        public StatusManager(IAutolootService autolootService)
        {
            _autolootService = autolootService;
        }
    }
}
