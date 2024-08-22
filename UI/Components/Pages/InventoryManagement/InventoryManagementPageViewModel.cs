using AutoMapper;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Jido.Services;

namespace Jido.UI.Components.Pages.InventoryManagement
{
    public partial class InventoryManagementPageViewModel : ViewModelBase
    {
        private Window? _inventoryOverlay;

        public InventoryManagementPageViewModel()
        {
        }

        public InventoryManagementPageViewModel(IAutopressService autopressService, IMapper mapper)
        {
        }

        #region commands

        [RelayCommand]
        private void ConfigureInventoryLayout()
        {
            if (_inventoryOverlay == null)
            {
                _inventoryOverlay = new InventoryOverlay(250, 250);
                _inventoryOverlay.Show();
            }
            else
            {
                _inventoryOverlay.Close();
                _inventoryOverlay = null;
            }
        }

        #endregion commands

        public void Dispose()
        {
            if (_inventoryOverlay != null)
            {
                _inventoryOverlay.Close();
            }
        }
    }
}
