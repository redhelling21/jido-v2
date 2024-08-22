using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Jido.UI.Components;
using Jido.UI.Components.Common.Sidebar;
using Jido.UI.Components.Pages.Autoloot;
using Jido.UI.Components.Pages.Autopress;
using Jido.Config;
using Jido.Services;
using Jido.UI.Components.Pages.Home;
using Jido.UI.Routing;
using Jido.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Jido.UI.Components.Pages.InventoryManagement;

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
                desktop.MainWindow = new MainWindow
                {
                    DataContext = services.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // Config
            services.AddSingleton<JidoConfig>(s => new JidoConfig("settings.json"));
            services.AddSingleton<Router<ViewModelBase>>(s => new Router<ViewModelBase>(t =>
                (ViewModelBase)s.GetRequiredService(t)
            ));
            services.AddSingleton<IHooksManager, HooksManager>();
            services.AddSingleton<IAutolootService, AutolootService>();
            services.AddSingleton<IAutopressService, AutopressService>();
            services.AddSingleton<IInventoryManagementService, InventoryManagementService>();

            // Component ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<SidebarViewModel>();
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<AutolootPageViewModel>();
            services.AddTransient<AutopressPageViewModel>();
            services.AddTransient<InventoryManagementPageViewModel>();

            // Utilities
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services.BuildServiceProvider();
        }
    }
}
