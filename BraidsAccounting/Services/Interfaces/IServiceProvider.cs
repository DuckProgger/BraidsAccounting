using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Models;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IServiceProvider
    {
        void ProvideService(IEnumerable<ServiceFormItem> serviceFormItems, string name, decimal profit);
    }
}