using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.ViewModels
{
    public static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainViewModel>()
            ;
    }
}
