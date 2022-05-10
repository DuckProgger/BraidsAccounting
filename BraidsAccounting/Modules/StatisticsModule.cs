using BraidsAccounting.Views;
using BraidsAccounting.Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules;

internal class StatisticsModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager
            .RegisterViewWithRegion(RegionNames.Statistics, typeof(WastedItemsView))
            .RegisterViewWithRegion(RegionNames.Statistics, typeof(PaymentsView))
            ;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<WastedItemsView>();
        containerRegistry.RegisterForNavigation<PaymentsView>();
    }
}
