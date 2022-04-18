using BraidsAccounting.Services.Interfaces;
using Prism.Ioc;

namespace BraidsAccounting.Services
{
    /// <summary>
    /// Класс регистрации сервисов в контейнере IoC.
    /// </summary>
    internal static class ServiceRegistrator
    {
        /// <summary>
        /// Добавить все используемые сервисы в контейнер IoC.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IContainerRegistry AddServices(this IContainerRegistry services) => services
            .Register<IServiceProvider, ServiceProvider>()
            .Register<IStoreService, StoreService>()
            .Register<IItemsService, ItemsService>()
            .Register<IViewService, ViewService>()
            .Register<IManufacturersService, ManufacturersService>()
            .Register<IStatisticsService, StatisticsService>()
            .Register<IEmployeesService, EmployeesService>()
           ;
    }
}
