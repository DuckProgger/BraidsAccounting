using BraidsAccounting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Modules
{
    internal class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager
                 .RegisterViewWithRegion("ContentRegion", typeof(StoreView))
                 .RegisterViewWithRegion("ContentRegion", typeof(ServiceView))
            //     .RegisterViewWithRegion("StoreItemRegion", typeof(AddStoreItemView))
            //.RegisterViewWithRegion("StoreItemRegion", typeof(EditStoreItemView))
            ;

        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<StoreView>();
            containerRegistry.RegisterForNavigation<StoreView>();
            containerRegistry.RegisterForNavigation<ServiceView>();

        }
    }
}
