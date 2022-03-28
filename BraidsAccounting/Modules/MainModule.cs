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
                 .RegisterViewWithRegion("ContentRegion", typeof(StoreView))
                 .RegisterViewWithRegion("ContentRegion", typeof(ServiceView))
                 .RegisterViewWithRegion("ContentRegion", typeof(ManufacturersView))
                 .RegisterViewWithRegion("ContentRegion", typeof(StatisticsView))
            ;

        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StoreView>();
            containerRegistry.RegisterForNavigation<ServiceView>();
            containerRegistry.RegisterForNavigation<ManufacturersView>();
            containerRegistry.RegisterForNavigation<StatisticsView>();
        }
    }
}
