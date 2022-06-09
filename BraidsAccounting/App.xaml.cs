using BraidsAccounting.Data;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.ViewModels;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System.Windows;

namespace BraidsAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .AddDatabase()
                .AddServices()
                ;            
        }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void Initialize()
        {
            base.Initialize();
            UpdateDatabase();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog
                .AddModule(typeof(MainModule))
                .AddModule(typeof(StatisticsModule))
                .AddModule(typeof(CatalogsModule))
                .AddModule(typeof(PopupModule))
                ;
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<PopupWindow, PopupViewModel>();
        }

        private static void UpdateDatabase()
        {
            var migrationService = ServiceLocator.GetService<IMigrationExecutorService>();
            migrationService.UpdateDatabase();
        }
    }
}
