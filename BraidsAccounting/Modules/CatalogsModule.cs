using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules
{
    internal class CatalogsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager
                .RegisterViewWithRegion(RegionNames.Store, typeof(StoreView))
                .RegisterViewWithRegion(RegionNames.Catalog, typeof(CatalogueView))
                .RegisterViewWithRegion(RegionNames.Manufacturers, typeof(ManufacturersView))
                .RegisterViewWithRegion(RegionNames.Employees, typeof(EmployeesView))
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
}
