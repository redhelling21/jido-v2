using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jido.Routing;
using Jido.Services;
using Jido.Utils;

namespace Jido.Components.Common.Sidebar
{
    public partial class SidebarViewModel : ViewModelBase
    {
        private Router<ViewModelBase> _router = default!;

        [ObservableProperty]
        private ServiceStatus _autolootStatus = ServiceStatus.STOPPED;

        [RelayCommand]
        public void NavigateTo(string path)
        {
            _router.GoTo(path);
        }

        public SidebarViewModel()
        {
            Console.WriteLine("SidebarViewModel created");
        }

        public SidebarViewModel(IAutolootService autolootService, Router<ViewModelBase> router)
        {
            _router = router;
            autolootService.StatusChanged += (sender, e) => AutolootStatus = e;
        }
    }
}
