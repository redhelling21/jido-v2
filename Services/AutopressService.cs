using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Jido.Config;
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
        private CancellationTokenSource _cancellationTokenSource;
        private JidoConfig _config;
        private KeyCode _toggleKey;
        private List<Color> _colors;

        public KeyCode ToggleKey
        {
            get => _toggleKey;
        }

        public List<Color> Colors
        {
            get => _colors;
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
                Task.Run(() => AutopressRoutine(_cancellationTokenSource.Token));
            }
            else
            {
                _cancellationTokenSource.Cancel();
                Status = ServiceStatus.STOPPED;
            }
            StatusChanged?.Invoke(this, Status);
        }

        private async Task AutopressRoutine(CancellationToken cancellationToken)
        { }
    }

    public interface IAutopressService : IServiceWithStatus
    {
        public Task<KeyCode> ChangeToggleKey();

        public KeyCode ToggleKey { get; }
    }
}
