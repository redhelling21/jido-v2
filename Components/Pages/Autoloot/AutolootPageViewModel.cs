using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Services;
using Jido.Utils;

namespace Jido.Components.Pages.Autoloot
{
    public partial class AutolootPageViewModel : ViewModelBase
    {
        private readonly IAutolootService? _autolootService;

        [ObservableProperty]
        private string changeKeyButtonText;

        [ObservableProperty]
        private string toggleKey;

        public AutolootPageViewModel()
        {
            ChangeKeyButtonText = "Change";
        }

        public AutolootPageViewModel(IAutolootService autolootService)
        {
            _autolootService = autolootService;
            _autolootService.StatusChanged += OnAutolootStatusChange;

            ChangeKeyButtonText = "Change";
        }

        private void OnAutolootStatusChange(object? sender, ServiceStatus status)
        {
        }

        [RelayCommand]
        private void ChangeKey()
        {
            if (_autolootService is not null)
            {
                ChangeKeyButtonText = "Listening...";
                var task = _autolootService.ChangeToggleKey();
                task.ContinueWith((key) =>
                {
                    ToggleKey = key.Result.ToString();
                    ChangeKeyButtonText = "Change";
                });
            }
        }
    }
}
