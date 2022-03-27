using BraidsAccounting.Services.Interfaces;
using Prism.Ioc;

namespace BraidsAccounting.Services
{
    internal static class ServiceRegistrator
    {
        public static IContainerRegistry AddServices(this IContainerRegistry services) => services
            .Register<Interfaces.IServiceProvider, ServiceProvider>()
            .Register<IStoreService, StoreService>()
            .Register<IItemsService, ItemsService>()
            .Register<IViewService, ViewService>()
            .Register<IManufacturersService, ManufacturersService>()
            .Register<IStatisticsService, StatisticsService>()
           ;
    }
}
