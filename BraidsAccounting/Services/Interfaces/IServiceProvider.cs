using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IServiceProvider
    {
        IEnumerable<string> GetNames();
        void ProvideService(Service service);
    }
}