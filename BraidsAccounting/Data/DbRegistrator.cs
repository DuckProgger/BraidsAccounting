using BraidsAccounting.DAL.Context;
using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System.IO;

namespace BraidsAccounting.Data
{
    internal static class DbRegistrator
    {
        //public static IServiceCollection AddDatabase(this IServiceCollection services) => services
        //    .AddDbContext<ApplicationContext>(opt =>
        //    {
        //        opt.UseSqlServer(GetConnectionString());
        //    })
        //    .AddRepositories()
        //    ;

        public static IContainerRegistry AddDatabase(this IContainerRegistry container) => container
            .Register<DbContext, ApplicationContext>()
            .AddRepositories()
            ;


       
    }
}
