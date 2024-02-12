using System;
using System.Collections.Generic;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Components.Common.Sidebar;
using Jido.Components.Pages.Autoloot;
using Jido.Components.Pages.Home;
using Jido.Routing;
using Jido.Services;
using Jido.Utils;

namespace Jido.Components
{
    public partial class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region Observable properties

        [ObservableProperty]
        private ViewModelBase _content = default!;

        [ObservableProperty]
        private string _pageKey = "Home";

        #endregion Observable properties

        private IKeyHooksManager? _hooksManager;

        public SidebarViewModel Sidebar { get; set; } = new();

        public MainWindowViewModel()
        {
            _content = new HomePageViewModel();
        }

        public MainWindowViewModel(Router<ViewModelBase> router, SidebarViewModel sidebar, IKeyHooksManager hooksManager)
        {
            Sidebar = sidebar;
            // register route changed event to set content to viewModel, whenever a route changes
            router.CurrentViewModelChanged += viewModel => Content = viewModel;
            _hooksManager = hooksManager;

            // change to HomeView
            router.GoTo<HomePageViewModel>();
        }

        public void OnClosing(object? sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            _hooksManager?.Dispose();
        }
    }
}
