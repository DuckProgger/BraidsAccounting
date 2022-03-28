using Prism.Regions;
using System.Linq;

namespace BraidsAccounting.Infrastructure
{
    /// <summary>
    /// Класс расширений для IRegionManager.
    /// </summary>
    internal static class RegionManagerExtensions
    {
        /// <summary>
        /// Определяет находится ли в активном состоянии представление, заданное типом <typeparamref name = "TViewType"/> .
        /// </summary>
        /// <typeparam name="TViewType">Тип представления.</typeparam>
        /// <param name="regionManager"></param>
        /// <param name="regionName">Имя региона.</param>
        /// <returns></returns>
        public static bool IsViewActive<TViewType>(this IRegionManager regionManager, string regionName)
        {
            IRegion? region = regionManager.Regions[regionName];
            object? view = region.Views.FirstOrDefault(v => v.GetType() == typeof(TViewType));
            return region.ActiveViews.Contains(view);
        }
    }
}
