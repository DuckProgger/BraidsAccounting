using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IServiceProvider
    {
        void ProvideService(Service service);
    }
}