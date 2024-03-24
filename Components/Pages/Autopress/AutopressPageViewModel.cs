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

        public ObservableCollection<Color> ColorItems { get; } = new ObservableCollection<Color>();

        public AutopressPageViewModel()
        {
            ChangeKeyButtonText = "Change";
            ColorItems = new ObservableCollection<Color>(
                new List<Color>()
                {
                    new() { Name = "Red", RGB = [255, 0, 0] },
                    new() { Name = "Green", RGB = [0, 255, 0] },
                    new() { Name = "Blue", RGB = [0, 0, 255] },
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
