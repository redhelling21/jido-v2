using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Models;
using Jido.Services;
using Jido.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jido.Components.Pages.Autoloot
{
    public partial class AutolootPageViewModel : ViewModelBase
    {
        private readonly IAutolootService? _autolootService;

        [ObservableProperty]
        private string changeKeyButtonText;

        [ObservableProperty]
        private string toggleKey;

        public ObservableCollection<Color> ColorItems { get; } = new ObservableCollection<Color>();

        public AutolootPageViewModel()
        {
            ChangeKeyButtonText = "Change";
        }

        public AutolootPageViewModel(IAutolootService autolootService)
        {
            _autolootService = autolootService;
            _autolootService.StatusChanged += OnAutolootStatusChange;
            ToggleKey = _autolootService.ToggleKey.ToString();
            foreach (var color in _autolootService.Colors)
            {
                ColorItems.Add(color);
            }
            ChangeKeyButtonText = "Change";
        }

        private void OnAutolootStatusChange(object? sender, ServiceStatus status)
        { }

        [RelayCommand]
        private void ChangeKey()
        {
            if (_autolootService is not null)
            {
                ChangeKeyButtonText = "Listening...";
                var task = _autolootService.ChangeToggleKey();
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
