using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using static System.Net.Mime.MediaTypeNames;

namespace Jido.Components.Common.Sidebar;

public partial class SidebarMenuItem : Button
{
    public static readonly StyledProperty<string> MenuNameProperty = AvaloniaProperty.Register<SidebarMenuItem, string>(
        nameof(Text)
    );

    public static readonly StyledProperty<string> MenuIconProperty = AvaloniaProperty.Register<SidebarMenuItem, string>(
        nameof(MenuIcon)
    );

    public static readonly StyledProperty<string> PathProperty = AvaloniaProperty.Register<SidebarMenuItem, string>(
        nameof(Path)
    );

    public string MenuName
    {
        get => GetValue(MenuNameProperty);
        set => SetValue(MenuNameProperty, value);
    }

    public string MenuIcon
    {
        get => GetValue(MenuIconProperty);
        set => SetValue(MenuIconProperty, value);
    }

    public string Path
    {
        get => GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }
}