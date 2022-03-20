using BraidsAccounting.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal static class ServiceRegistrator
    {
        //public static IServiceCollection AddServices(this IServiceCollection services) => services
        //    .AddTransient<Interfaces.IServiceProvider, ServiceProvider>()
        //    .AddTransient<IStoreService, StoreService>()
        //    ;

        public static IContainerRegistry AddServices(this IContainerRegistry services) => services
           .Register<Interfaces.IServiceProvider, ServiceProvider>()
           .Register<IStoreService, StoreService>()
           ;


        
    }
}
