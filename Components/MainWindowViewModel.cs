using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Components.Pages.Autoloot;
using Jido.Components.Pages.Home;
using Jido.Utils;

namespace Jido.Components
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _content = default!;

        private Router<ViewModelBase>? _router;

        [RelayCommand]
        public void GoToHomePage() => _router?.GoTo<HomePageViewModel>();

        [RelayCommand]
        public void GoToAutolootPage() => _router?.GoTo<AutolootPageViewModel>();

        public MainWindowViewModel()
        {
            _content = new HomePageViewModel();
        }

        public MainWindowViewModel(Router<ViewModelBase> router)
        {
            _router = router;
            // register route changed event to set content to viewModel, whenever a route changes
            router.CurrentViewModelChanged += viewModel => Content = viewModel;

            // change to HomeView
            router.GoTo<HomePageViewModel>();
        }
    }
}