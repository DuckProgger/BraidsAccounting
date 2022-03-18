using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BraidsAccounting.DAL.Repositories
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddTransient<IRepository<Item>, DbRepository<Item>>()
            .AddTransient<IRepository<Service>, ServiceRepository>()
            .AddTransient<IRepository<StoreItem>, StoreRepository>()
            ;
    }
}
