using BraidsAccounting.Views;
using BraidsAccounting.Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules;

internal class PopupModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager

            .RegisterViewWithRegion(RegionNames.Popup, typeof(EditItemView))
            .RegisterViewWithRegion(RegionNames.Popup, typeof(AddItemView))
            ;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<EditItemView>();
        containerRegistry.RegisterForNavigation<EditStoreItemView>();
        containerRegistry.RegisterForNavigation<AddItemView>();
        containerRegistry.RegisterForNavigation<AddStoreItemView>();
        containerRegistry.RegisterForNavigation<CatalogueView>();
        containerRegistry.RegisterForNavigation<SelectItemView>();
        containerRegistry.RegisterForNavigation<SelectStoreItemView>();
    }
}
