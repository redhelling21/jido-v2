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
using static Jido.Models.CompositeHighLevelCommand;

namespace Jido.Components.Pages.Autopress
{
    public partial class AutopressPageViewModel : ViewModelBase
    {
        private readonly IAutopressService? _autopressService;

        [ObservableProperty]
        private string changeKeyButtonText;

        [ObservableProperty]
        private string toggleKey;

        [ObservableProperty]
        private int clickDelay;

        public ObservableCollection<HighLevelCommand> ScheduledCommands { get; } =
            new ObservableCollection<HighLevelCommand>();

        public ObservableCollection<ConstantCommand> ConstantCommands { get; } =
            new ObservableCollection<ConstantCommand>();

        public AutopressPageViewModel()
        {
            ChangeKeyButtonText = "Change";
            // Placeholder for design purpose
            ScheduledCommands = new ObservableCollection<HighLevelCommand>(
                new List<HighLevelCommand>()
                {
                    new BasicHighLevelCommand(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcE }, 1000),
                    new BasicHighLevelCommand(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcR }, 2000),
                    new BasicHighLevelCommand(new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcR }, 3000),
                    new CompositeHighLevelCommand(
                        new ObservableCollection<LowLevelCommand>()
                            {
                                new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcW },
                                new WaitCommand() { WaitTimeInMs = 500 },
                                new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcO },
                            },
                        1500
                    ),
                    new CompositeHighLevelCommand(
                        new ObservableCollection<LowLevelCommand>()
                            {
                                new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcT },
                                new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcB }
                            },
                        2500
                    )
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
            ConstantCommands = new ObservableCollection<ConstantCommand>();
        }

        private void OnAutopressStatusChange(object? sender, ServiceStatus status)
        { }

        partial void OnClickDelayChanged(int value)
        {
            if (_autopressService is not null)
            {
                _autopressService.UpdateClickDelay(value);
            }
        }

        #region commands

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

        [RelayCommand]
        private void AddConstantCommand()
        {
            var command = new ConstantCommand();
            ConstantCommands.Add(command);
        }

        [RelayCommand]
        private void RemoveConstantCommand(ConstantCommand command)
        {
            ConstantCommands.Remove(command);
        }

        [RelayCommand]
        private void AddBasicHighLevelCommand()
        {
            var command = new BasicHighLevelCommand(
                new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcUndefined },
                1000
            );
            ScheduledCommands.Add(command);
        }

        [RelayCommand]
        private void AddCompositeHighLevelCommand()
        {
            var command = new CompositeHighLevelCommand(
                new ObservableCollection<LowLevelCommand>()
                    {
                        new PressCommand() { KeyToPress = SharpHook.Native.KeyCode.VcUndefined },
                    },
                1000
            );
            ScheduledCommands.Add(command);
        }

        [RelayCommand]
        private void AddLowLevelCommandToComposite(CompositeHighLevelCommand command)
        {
        }

        [RelayCommand]
        private void RemoveLowLevelCommandFromComposite(LowLevelCommand command)
        {
            var parent = ScheduledCommands.Where(c =>
            {
                return c is CompositeHighLevelCommand && ((CompositeHighLevelCommand)c).Commands.Contains(command);
            }).FirstOrDefault();
            if (parent != null)
            {
                ((CompositeHighLevelCommand)parent).Commands.Remove(command);
            }
        }

        [RelayCommand]
        private void RemoveHighLevelCommand(HighLevelCommand command)
        {
            ScheduledCommands.Remove(command);
        }

        [RelayCommand]
        private void MoveUpHighLevelCommand(HighLevelCommand command)
        {
            var index = ScheduledCommands.IndexOf(command);
            if (index > 0)
            {
                ScheduledCommands.Move(index, index - 1);
            }
        }

        [RelayCommand]
        private void MoveDownHighLevelCommand(HighLevelCommand command)
        {
            var index = ScheduledCommands.IndexOf(command);
            if (index < ScheduledCommands.Count - 1)
            {
                ScheduledCommands.Move(index, index + 1);
            }
        }

        [RelayCommand]
        private void SaveHighLevelCommands()
        {
            if (_autopressService is not null)
            {
                _autopressService.UpdateScheduledCommands(ScheduledCommands.ToList());
            }
        }

        [RelayCommand]
        private void SaveConstantCommands()
        {
            if (_autopressService is not null)
            {
                _autopressService.UpdateConstantCommands(ConstantCommands.ToList());
            }
        }

        #endregion commands
    }
}
