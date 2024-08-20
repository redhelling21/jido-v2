using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Jido.Config;
using Jido.Models;
using Jido.Utils;
using OpenCvSharp;
using SharpHook;
using SharpHook.Native;
using static Jido.Models.CompositeHighLevelCommand;
using Color = Jido.Models.Color;
using Point = OpenCvSharp.Point;

namespace Jido.Services
{
    public class AutopressService : IAutopressService
    {
        private IKeyHooksManager _keyHooksManager;
        private EventSimulator _eventSimulator = new EventSimulator();
        private CancellationTokenSource _cancellationTokenSource;
        private JidoConfig _config;
        private KeyCode _toggleKey;
        private List<HighLevelCommand> _scheduledCommands;
        private List<ConstantCommand> _constantCommands;
        private int _clickDelay;
        private ConcurrentQueue<LowLevelCommand> _queuedCommands = new ConcurrentQueue<LowLevelCommand>();

        public KeyCode ToggleKey => _toggleKey;
        public List<HighLevelCommand> ScheduledCommands => _scheduledCommands;
        public List<ConstantCommand> ConstantCommands => _constantCommands;
        public int ClickDelay => _clickDelay;

        public ServiceStatus Status { get; set; } = ServiceStatus.STOPPED;

        public event EventHandler<ServiceStatus> StatusChanged;

        public AutopressService(IKeyHooksManager keyHooksManager, JidoConfig config)
        {
            _keyHooksManager = keyHooksManager;
            _config = config;
            _toggleKey = _config.Features.Autopress.ToggleKey;
            _scheduledCommands = _config.Features.Autopress.ScheduledCommands;
            _constantCommands = _config.Features.Autopress.ConstantCommands;
            _keyHooksManager.RegisterKey(_toggleKey, ToggleAutopress);
        }

        public Task<KeyCode> ChangeToggleKey()
        {
            var task = _keyHooksManager
                .ListenNextKey()
                .ContinueWith(
                    (key) =>
                    {
                        _keyHooksManager.UnregisterKey(_toggleKey);
                        _toggleKey = key.Result;
                        _config.Features.Autopress.ToggleKey = key.Result;
                        _config.Persist();
                        _keyHooksManager.RegisterKey(_toggleKey, ToggleAutopress);
                        return _toggleKey;
                    }
                );
            return task;
        }

        public void UpdateScheduledCommands(List<HighLevelCommand> commands)
        {
            _scheduledCommands = commands;
            _config.Features.Autopress.ScheduledCommands = commands;
            _config.Persist();
        }

        public void UpdateConstantCommands(List<ConstantCommand> commands)
        {
            _constantCommands = commands;
            _config.Features.Autopress.ConstantCommands = commands;
            _config.Persist();
        }

        public void UpdateClickDelay(int delay)
        {
            _clickDelay = delay;
            _config.Features.Autopress.ClickDelay = delay;
            _config.Persist();
        }

        private void ToggleAutopress(object? sender, EventArgs e)
        {
            if (Status == ServiceStatus.STOPPED)
            {
                Status = ServiceStatus.IDLE;
                _cancellationTokenSource = new CancellationTokenSource();
                StartAutoPress();
            }
            else
            {
                StopAutoPress();
                Status = ServiceStatus.STOPPED;
            }
            StatusChanged?.Invoke(this, Status);
        }

        private void StartAutoPress()
        {
            // Setup
            foreach (var command in _constantCommands)
            {
                _eventSimulator.SimulateKeyPress(command.KeyToPress);
            }

            // Start key press routine
            _ = Task.Run(() => KeyPressRoutine(_cancellationTokenSource.Token));

            foreach (var command in _scheduledCommands)
            {
                command.Start(_queuedCommands);
            }
        }

        private void StopAutoPress()
        {
            _cancellationTokenSource.Cancel();

            foreach (var command in _constantCommands)
            {
                _eventSimulator.SimulateKeyRelease(command.KeyToPress);
            }
        }

        private async Task KeyPressRoutine(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var foundWork = _queuedCommands.TryDequeue(out var command);
                if (foundWork)
                {
                    if (command is WaitCommand waitCommand)
                    {
                        await Task.Delay(waitCommand.WaitTimeInMs);
                    }
                    else if (command is PressCommand pressCommand)
                    {
                        await Task.Delay(300);
                        _eventSimulator.SimulateKeyPress(pressCommand.KeyToPress);
                        await Task.Delay(pressCommand.PressDurationInMs);
                        _eventSimulator.SimulateKeyRelease(pressCommand.KeyToPress);
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
    }

    public interface IAutopressService : IServiceWithStatus, IToggleableService
    {
        public List<HighLevelCommand> ScheduledCommands { get; }
        public List<ConstantCommand> ConstantCommands { get; }

        public void UpdateScheduledCommands(List<HighLevelCommand> commands);

        public void UpdateConstantCommands(List<ConstantCommand> commands);

        public void UpdateClickDelay(int delay);
    }
}
