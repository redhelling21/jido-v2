using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Services;
using Jido.UI.Components;
using Jido.UI.Routing;
using Jido.Utils;

namespace Jido.UI.Components.Common.Sidebar
{
    public partial class SidebarViewModel : ViewModelBase
    {
        private Router<ViewModelBase> _router = default!;

        [ObservableProperty]
        private ServiceStatus _autolootStatus = ServiceStatus.STOPPED;

        [ObservableProperty]
        private ServiceStatus _autopressStatus = ServiceStatus.STOPPED;

        [RelayCommand]
        public void NavigateTo(string path)
        {
            _router.GoTo(path);
        }

        public SidebarViewModel()
        {
            Console.WriteLine("SidebarViewModel created");
        }

        public SidebarViewModel(
            IAutolootService autolootService,
            IAutopressService autopressService,
            Router<ViewModelBase> router
        )
        {
            _router = router;
            autolootService.StatusChanged += (sender, e) => AutolootStatus = e;
            autopressService.StatusChanged += (sender, e) => AutopressStatus = e;
        }
    }
}
