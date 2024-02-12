using System;
using System.Collections.Generic;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Components.Common.Sidebar;
using Jido.Components.Pages.Autoloot;
using Jido.Components.Pages.Home;
using Jido.Routing;

namespace Jido.Components
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Observable properties

        [ObservableProperty]
        private ViewModelBase _content = default!;

        [ObservableProperty]
        private string _pageKey = "Home";

        #endregion Observable properties

        public SidebarViewModel Sidebar { get; set; } = new();

        public MainWindowViewModel()
        {
            _content = new HomePageViewModel();
        }

        public MainWindowViewModel(Router<ViewModelBase> router, SidebarViewModel sidebar)
        {
            Sidebar = sidebar;
            // register route changed event to set content to viewModel, whenever a route changes
            router.CurrentViewModelChanged += viewModel => Content = viewModel;

            // change to HomeView
            router.GoTo<HomePageViewModel>();
        }
    }
}
