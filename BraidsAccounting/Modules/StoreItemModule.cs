using BraidsAccounting.ViewModels;
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
    internal class StoreItemModule : IModule
    {
        public const string RegionName = "StoreItemRegion";
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion("StoreItemRegion", typeof(AddStoreItemView));
            //regionManager.RegisterViewWithRegion("StoreItemRegion", typeof(EditStoreItemView));

            AddStoreItemView view1 = new();
            EditStoreItemView view2 = new();

            IRegion region = new Region();
            region.Name = RegionName;
            regionManager.Regions.Add(region);
            region.Add(view1);
            region.Add(view2);
            region.Activate(view1);
            //region.Activate(view2);

        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddStoreItemView>();
            containerRegistry.RegisterForNavigation<EditStoreItemView>();
        }
    }
}
