using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Repositories;
using Prism.Ioc;

namespace BraidsAccounting.Data
{
    internal static class DbRegistrator
    {
        public static IContainerRegistry AddDatabase(this IContainerRegistry container) => container
            .Register<ApplicationContext>(MyServiceFactory.CreateDbContext)
            .AddRepositories()
            ;
    }
}
