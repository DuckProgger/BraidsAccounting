using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using Prism.Ioc;

namespace BraidsAccounting.DAL.Repositories;

/// <summary>
/// Класс для регистрации репозиториев в контейнере IoC.
/// </summary>
public static class RepositoryRegistrator
{
    public static IContainerRegistry AddRepositories(this IContainerRegistry services) => services
       .Register<IRepository<Item>, ItemRepository>()
       .Register<IRepository<Service>, ServiceRepository>()
       .Register<IRepository<StoreItem>, StoreRepository>()
       .Register<IRepository<WastedItem>, WastedItemRepository>()
       .Register<IRepository<Manufacturer>, DbRepository<Manufacturer>>()
       .Register<IRepository<Employee>, DbRepository<Employee>>()
       .Register<IRepository<Payment>, DbRepository<Payment>>()
        ;
}
