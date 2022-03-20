using BraidsAccounting.Data;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.ViewModels;
using BraidsAccounting.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BraidsAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        //private static IHost? __Host;
        //public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        //public static IServiceProvider Services => Host.Services;

        ////protected override create

        //internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        //    .AddServices()
        //    .AddViewModels()
        //    .AddDatabase()
        //    ;

        //protected override async void OnStartup(StartupEventArgs e)
        //{
        //    IHost? host = Host;
        //    base.OnStartup(e);
        //    await host.StartAsync();
        //}
        //protected override async void OnExit(ExitEventArgs e)
        //{
        //    using IHost? host = Host;
        //    base.OnExit(e);
        //    await host.StopAsync();
        //}

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .AddDatabase()
                .AddServices()
                ;
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(typeof(MainModule));
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();    
            ViewModelLocationProvider.Register<AddStoreItemWindow, AddStoreItemViewModel>();
        }
    }
}
