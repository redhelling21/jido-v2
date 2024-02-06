using CommunityToolkit.Mvvm.ComponentModel;
using Jido.Components.Pages.Home;
using Jido.Utils;

namespace Jido.Components
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _content = default!;

        public MainWindowViewModel()
        {
            _content = new HomePageViewModel();
        }

        public MainWindowViewModel(Router<ViewModelBase> router)
        {
            // register route changed event to set content to viewModel, whenever a route changes
            router.CurrentViewModelChanged += viewModel => Content = viewModel;

            // change to HomeView
            router.GoTo<HomePageViewModel>();
        }
    }
}