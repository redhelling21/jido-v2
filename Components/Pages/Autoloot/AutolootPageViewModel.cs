using CommunityToolkit.Mvvm.ComponentModel;
using Jido.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        {
        }

        public AutolootPageViewModel(IAutolootService autolootService)
        {
            _autolootService = autolootService;
            _autolootService.PressEvent += OnKeyPressed;
            _autolootService.ReleaseEvent += OnKeyReleased;
        }

        private void OnKeyPressed(object? sender, EventArgs e)
        {
            IsKeyPressed = true;
            IsKeyReleased = false;
        }

        private void OnKeyReleased(object? sender, EventArgs e)
        {
            IsKeyPressed = false;
            IsKeyReleased = true;
        }
    }
}
