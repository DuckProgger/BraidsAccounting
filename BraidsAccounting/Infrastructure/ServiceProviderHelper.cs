using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure
{
    public static class ServiceProviderHelper
    {
        public static T? GetService<T>() where T : class
        {
            return App.Services.GetService(typeof(T)) as T;
        }
    }
}
