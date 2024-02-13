using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Jido.Services;
using Jido.Utils;

namespace Jido.Components.Pages.Autoloot
{
    public partial class AutolootPageViewModel : ViewModelBase
    {
        private readonly IAutolootService? _autolootService;

        [ObservableProperty]
        private bool isKeyPressed = false;

        [ObservableProperty]
        private bool isKeyReleased = true;

        public AutolootPageViewModel()
        { }

        public AutolootPageViewModel(IAutolootService autolootService)
        {
            _autolootService = autolootService;
            _autolootService.AutolootStatusChanged += OnAutolootStatusChange;
            IsKeyPressed = (_autolootService.AutolootStatus & (ServiceStatus.RUNNING)) == ServiceStatus.RUNNING;
        }

        private void OnAutolootStatusChange(object? sender, ServiceStatus status)
        {
            IsKeyPressed = (status & (ServiceStatus.RUNNING)) == ServiceStatus.RUNNING;
        }
    }
}