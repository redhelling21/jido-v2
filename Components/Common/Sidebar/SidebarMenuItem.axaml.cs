using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;

namespace Jido.Components.Common.Sidebar;

public partial class SidebarMenuItem : TemplatedControl
{
    public static readonly StyledProperty<string> MenuNameProperty =
            AvaloniaProperty.Register<SidebarMenuItem, string>(nameof(Text));

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<SidebarMenuItem, ICommand>(nameof(Command));

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<SidebarMenuItem, object>(nameof(CommandParameter));

    public static readonly StyledProperty<string> MenuIconProperty =
        AvaloniaProperty.Register<SidebarMenuItem, string>(nameof(MenuIcon));

    public static readonly StyledProperty<string> PathProperty =
        AvaloniaProperty.Register<SidebarMenuItem, string>(nameof(Path));

    public string MenuName
    {
        get => GetValue(MenuNameProperty);
        set => SetValue(MenuNameProperty, value);
    }

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
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