using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Services
{
    internal class ServiceProvider : Interfaces.IServiceProvider
    {
        private readonly IRepository<Service> services;

        public ServiceProvider(IRepository<Service> services)
        {
            this.services = services;
        }

        public void ProvideService(Service service)
        {

        }
    }
}
