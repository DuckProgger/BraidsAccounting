using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace BraidsAccounting.DAL.Repositories
{
    public static class RepositoryRegistrator
    {
        //public static IServiceCollection AddRepositories(this IServiceCollection services) => services
        //    .AddTransient<IRepository<Item>, ItemRepository>()
        //    .AddTransient<IRepository<Service>, ServiceRepository>()
        //    .AddTransient<IRepository<StoreItem>, StoreRepository>()
        //    .AddTransient<IRepository<WastedItem>, WastedItemRepository>()
        //    .AddTransient<IRepository<ItemPrice>, DbRepository<ItemPrice>>()

        //    ;

        public static IContainerRegistry AddRepositories(this IContainerRegistry services) => services
           .Register<IRepository<Item>, ItemRepository>()
           .Register<IRepository<Service>, ServiceRepository>()
           .Register<IRepository<StoreItem>, StoreRepository>()
           .Register<IRepository<WastedItem>, WastedItemRepository>()
           .Register<IRepository<Manufacturer>, DbRepository<Manufacturer>>()
           ;
    }
}
