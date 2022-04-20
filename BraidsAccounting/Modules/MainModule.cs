using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules
{
    /// <summary>
    /// Представляет модуль для навигации по представлениям из главного экрана.
    /// </summary>
    internal class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager
                 .RegisterViewWithRegion(RegionNames.Main, typeof(StoreView))
                 .RegisterViewWithRegion(RegionNames.Main, typeof(ServiceView))
                 .RegisterViewWithRegion(RegionNames.Main, typeof(ManufacturersView))
                 .RegisterViewWithRegion(RegionNames.Main, typeof(StatisticsView))
                 .RegisterViewWithRegion(RegionNames.Main, typeof(EmployeesView))
            ;

        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StoreView>();
            containerRegistry.RegisterForNavigation<ServiceView>();
            containerRegistry.RegisterForNavigation<ManufacturersView>();
            containerRegistry.RegisterForNavigation<StatisticsView>();
            containerRegistry.RegisterForNavigation<EmployeesView>();
        }
    }
}
