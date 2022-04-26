﻿using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BraidsAccounting.Modules
{
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
            containerRegistry.RegisterForNavigation<AddItemView>();

        }
    }
}
