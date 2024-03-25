using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Jido.Config;
using Jido.Models;
using Jido.Utils;
using OpenCvSharp;
using SharpHook;
using SharpHook.Native;
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
        private List<HighLevelCommand> _commands;
        private List<ConstantCommand> _constantCommands;
        private ConcurrentQueue<LowLevelCommand> _queuedCommands;

        public KeyCode ToggleKey
        {
            get => _toggleKey;
        }

        public ServiceStatus Status { get; set; } = ServiceStatus.STOPPED;

        public event EventHandler<ServiceStatus> StatusChanged;

        public AutopressService(IKeyHooksManager keyHooksManager, JidoConfig config)
        {
            _keyHooksManager = keyHooksManager;
            _config = config;
            _toggleKey = _config.Features.Autopress.ToggleKey;
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

        private void ToggleAutopress(object? sender, EventArgs e)
        {
            if (Status == ServiceStatus.STOPPED)
            {
                Status = ServiceStatus.IDLE;
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => MainRoutine(_cancellationTokenSource.Token));
            }
            else
            {
                _cancellationTokenSource.Cancel();
                Status = ServiceStatus.STOPPED;
            }
            StatusChanged?.Invoke(this, Status);
        }

        private async Task MainRoutine(CancellationToken cancellationToken)
        {
            // Setup
            foreach (var command in _constantCommands)
            {
                _eventSimulator.SimulateKeyPress(command.KeyToPress);
            }

            // Loops
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }

            // Teardown
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

    public interface IAutopressService : IServiceWithStatus
    {
        public Task<KeyCode> ChangeToggleKey();

        public KeyCode ToggleKey { get; }
    }
}
