using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Jido.Components.Common.Sidebar;

namespace Jido.Components.Common;

public class PageWithHeader : ContentControl
{
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<SidebarMenuItem, string>(
        nameof(Title)
    );

    public static readonly StyledProperty<string> SubtitleProperty = AvaloniaProperty.Register<SidebarMenuItem, string>(
        nameof(Subtitle)
    );

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
}