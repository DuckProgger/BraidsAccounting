using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules;

internal class CatalogsModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager
            .RegisterViewWithRegion(RegionNames.Catalogs, typeof(StoreView))
            .RegisterViewWithRegion(RegionNames.Catalogs, typeof(CatalogueView))
            .RegisterViewWithRegion(RegionNames.Catalogs, typeof(ManufacturersView))
            .RegisterViewWithRegion(RegionNames.Catalogs, typeof(EmployeesView))
            ;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<StoreView>();
        containerRegistry.RegisterForNavigation<CatalogueView>();
        containerRegistry.RegisterForNavigation<ManufacturersView>();
        containerRegistry.RegisterForNavigation<EmployeesView>();
    }
}
