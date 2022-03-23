using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;

namespace BraidsAccounting.Services
{
    internal static class ServiceLocator
    {
        public static T GetService<T>() => ContainerLocator.Container.Resolve<T>();
    }
}
