using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules
{
    internal class StatisticsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager
                .RegisterViewWithRegion(RegionNames.WastedItems, typeof(WastedItemsView))
                .RegisterViewWithRegion(RegionNames.Payments, typeof(PaymentsView))
                ;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WastedItemsView>();
            containerRegistry.RegisterForNavigation<PaymentsView>();
        }
    }
}
