using BraidsAccounting.Services.Interfaces;
using Prism.Ioc;

namespace BraidsAccounting.Services;

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
        .Register<ICatalogueService, CatalogueService>()
        .RegisterSingleton<IViewService, ViewService>()
        .Register<IManufacturersService, ManufacturersService>()
        .Register<IWastedItemsService, WastedItemsService>()
        .Register<IEmployeesService, EmployeesService>()
        .Register<IPaymentsService, PaymentsService>()
        .RegisterSingleton<IHistoryService, HistoryService>()
       ;
}
