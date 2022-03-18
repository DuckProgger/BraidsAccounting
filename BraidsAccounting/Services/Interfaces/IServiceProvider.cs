using BraidsAccounting.DAL.Entities;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IServiceProvider
    {
        void ProvideService(Service service);
    }
}