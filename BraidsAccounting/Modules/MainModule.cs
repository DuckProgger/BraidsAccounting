using BraidsAccounting.Views;
using BraidsAccounting.Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules;

/// <summary>
/// Представляет модуль для навигации по представлениям из главного экрана.
/// </summary>
internal class MainModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager
             .RegisterViewWithRegion(RegionNames.Main, typeof(CatalogsView))
             .RegisterViewWithRegion(RegionNames.Main, typeof(ServiceView))
             .RegisterViewWithRegion(RegionNames.Main, typeof(StatisticsView))
             .RegisterViewWithRegion(RegionNames.Main, typeof(HistoryView))
        ;

    }
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<CatalogsView>();
        containerRegistry.RegisterForNavigation<ServiceView>();
        containerRegistry.RegisterForNavigation<StatisticsView>();
        containerRegistry.RegisterForNavigation<HistoryView>();
    }
}
