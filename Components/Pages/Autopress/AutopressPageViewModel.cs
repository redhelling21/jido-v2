using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Models;
using Jido.Services;
using Jido.Utils;

namespace Jido.Components.Pages.Autopress
{
    public partial class AutopressPageViewModel : ViewModelBase
    {
        private readonly IAutopressService? _autopressService;

        [ObservableProperty]
        private string changeKeyButtonText;

        [ObservableProperty]
        private string toggleKey;

        public ObservableCollection<HighLevelCommand> ScheduledCommands { get; } =
            new ObservableCollection<HighLevelCommand>();

        public ObservableCollection<ConstantCommand> ConstantCommands { get; } =
            new ObservableCollection<ConstantCommand>();

        public AutopressPageViewModel()
        {
            ChangeKeyButtonText = "Change";
            ScheduledCommands = new ObservableCollection<HighLevelCommand>(
                new List<HighLevelCommand>()
                {
                    new(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcE }, 1000),
                    new(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcR }, 2000),
                    new(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcZ }, 4000)
                }
            );
            ConstantCommands = new ObservableCollection<ConstantCommand>(
                new List<ConstantCommand>()
                {
                    new() { KeyToPress = SharpHook.Native.KeyCode.VcY },
                    new() { KeyToPress = SharpHook.Native.KeyCode.VcH },
                }
            );
        }

        public AutopressPageViewModel(IAutopressService autopressService)
        {
            _autopressService = autopressService;
            _autopressService.StatusChanged += OnAutopressStatusChange;
            ToggleKey = _autopressService.ToggleKey.ToString();
            ChangeKeyButtonText = "Change";
        }

        private void OnAutopressStatusChange(object? sender, ServiceStatus status)
        { }

        [RelayCommand]
        private void ChangeKey()
        {
            if (_autopressService is not null)
            {
                ChangeKeyButtonText = "Listening...";
                var task = _autopressService.ChangeToggleKey();
                task.ContinueWith(
                    (key) =>
                    {
                        ToggleKey = key.Result.ToString();
                        ChangeKeyButtonText = "Change";
                    }
                );
            }
        }
    }
}
