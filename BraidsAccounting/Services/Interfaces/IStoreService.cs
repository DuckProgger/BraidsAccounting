using BraidsAccounting.DAL.Entities;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IStoreService
    {
        void AddItem(StoreItem storeItem);
    }
}