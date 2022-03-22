using DryIoc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure
{
    internal static class Utils
    {
        public static bool IsViewActive<TViewType>(this IRegionManager regionManager, string regionName)
        {
            var region = regionManager.Regions[regionName];
            var view = region.Views.FirstOrDefault(v => v.GetType() == typeof(TViewType));
            return region.ActiveViews.Contains(view);
        }
    }
}
