using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Jido.Routing;

namespace Jido.Components.Common.Sidebar
{
    public partial class SidebarViewModel : ViewModelBase
    {
        private Router<ViewModelBase> _router = default!;

        [RelayCommand]
        public void NavigateTo(SidebarMenuItem item)
        {
            _router.GoTo(item.Path);
        }

        public SidebarViewModel()
        {
            Console.WriteLine("SidebarViewModel created");
        }

        public SidebarViewModel(Router<ViewModelBase> router)
        {
            _router = router;
        }
    }
}