using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IStoreService
    {
        void AddItem(StoreItem storeItem);
        IEnumerable<StoreItem> GetItems();
        void RemoveItems(IEnumerable<WastedItem> items);
    }
}