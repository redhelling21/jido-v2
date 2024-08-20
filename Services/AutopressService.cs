using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jido.Config;
using Jido.Models;
using Jido.Utils;
using SharpHook;
using SharpHook.Native;

namespace Jido.Services
{
    public class AutopressService : IAutopressService
    {
        private IHooksManager _keyHooksManager;
        private EventSimulator _eventSimulator = new EventSimulator();
        private CancellationTokenSource _cancellationTokenSource;
        private JidoConfig _config;
        private KeyCode _toggleKey;
        private List<HighLevelCommand> _scheduledCommands;
        private List<ConstantCommand> _constantCommands;
        private int _clickDelay;
        private double _intervalsRandomizationRatio;
        private ConcurrentQueue<LowLevelCommand> _queuedCommands = new ConcurrentQueue<LowLevelCommand>();
        private System.Timers.Timer _suspendTimer = new System.Timers.Timer();

        public KeyCode ToggleKey => _toggleKey;
        public List<HighLevelCommand> ScheduledCommands => _scheduledCommands;
        public List<ConstantCommand> ConstantCommands => _constantCommands;
        public int ClickDelay => _clickDelay;
        public double IntervalRandomizationRatio => _intervalsRandomizationRatio;
        private ServiceStatus _status = ServiceStatus.STOPPED;

        public ServiceStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<ServiceStatus> StatusChanged;

        public AutopressService(IHooksManager keyHooksManager, JidoConfig config)
        {
            _keyHooksManager = keyHooksManager;
            _config = config;
            InitFromConfig();
            _keyHooksManager.RegisterKey(_toggleKey, ToggleAutopress);
            _keyHooksManager.RegisterMouseClick(MouseButton.Button1, SuspendAutoPress);
        }

        private void InitFromConfig()
        {
            _toggleKey = _config.Features.Autopress.ToggleKey;
            _scheduledCommands = _config.Features.Autopress.ScheduledCommands;
            _constantCommands = _config.Features.Autopress.ConstantCommands;
            _clickDelay = _config.Features.Autopress.ClickDelay;
        }

        public void SuspendAutoPress(object? sender, EventArgs e)
        {
            if (Status == ServiceStatus.IDLE || Status == ServiceStatus.WORKING)
            {
                StopAutoPress();
                Status = ServiceStatus.PAUSED;
                _suspendTimer = new System.Timers.Timer(ClickDelay);
                _suspendTimer.Elapsed += (sender, e) =>
                {
                    StartAutoPress();
                };
                _suspendTimer.AutoReset = false;
                _suspendTimer.Start();
            }
            else if (Status == ServiceStatus.PAUSED)
            {
                _suspendTimer.Interval = ClickDelay;
            }
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

        public void UpdateConfig(AutopressConfig config)
        {
            if (Status == ServiceStatus.IDLE || Status == ServiceStatus.WORKING)
            {
                StopAutoPress();
            }
            config.ToggleKey = ToggleKey;
            _config.Features.Autopress = config;
            _config.Persist();
            InitFromConfig();
        }

        private void ToggleAutopress(object? sender, EventArgs e)
        {
            if (Status == ServiceStatus.STOPPED)
            {
                StartAutoPress();
            }
            else
            {
                StopAutoPress();
            }
        }

        private void StartAutoPress()
        {
            _cancellationTokenSource = new CancellationTokenSource();
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

            Status = ServiceStatus.IDLE;
        }

        private void StopAutoPress()
        {
            _cancellationTokenSource.Cancel();

            foreach (var command in _constantCommands)
            {
                _eventSimulator.SimulateKeyRelease(command.KeyToPress);
            }
            foreach (var command in _scheduledCommands)
            {
                command.Stop();
            }
            Status = ServiceStatus.STOPPED;
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

        public int ClickDelay { get; }

        public double IntervalRandomizationRatio { get; }

        public void UpdateConfig(AutopressConfig config);
    }
}
