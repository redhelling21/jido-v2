using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;

namespace Jido.UI.Components.Common.Sidebar;

public partial class SidebarView : UserControl
{
    private SidebarMenuItem _selectedItem;

    public SidebarView()
    {
        InitializeComponent();
        _selectedItem = this.FindControl<SidebarMenuItem>("Default");
    }

    public void SetActiveState(object sender, RoutedEventArgs args)
    {
        SidebarMenuItem item = (SidebarMenuItem)sender;
        if (item is null)
        {
            return;
        }
        if (_selectedItem != null)
        {
            _selectedItem.Classes.Remove("active");
        }
        item.Classes.Add("active");
        _selectedItem = item;
    }
}