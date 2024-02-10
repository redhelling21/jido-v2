using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Jido.Components;
using Jido.Components.Common.Sidebar;
using Jido.Components.Pages.Autoloot;
using Jido.Components.Pages.Home;
using Jido.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Jido
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IServiceProvider services = ConfigureServices();
            var router = services.GetRequiredService<Router<ViewModelBase>>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation. Without this line you
                // will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(router), };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // Routing
            services.AddSingleton<Router<ViewModelBase>>(s => new Router<ViewModelBase>(t =>
                (ViewModelBase)s.GetRequiredService(t)
            ));
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<AutolootPageViewModel>();

            return services.BuildServiceProvider();
        }
    }
}