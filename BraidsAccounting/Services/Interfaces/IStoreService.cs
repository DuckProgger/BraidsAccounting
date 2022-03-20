﻿using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IStoreService
    {
        void AddItem(StoreItem? storeItem);
        void EditItem(StoreItem? storeItem);
        IEnumerable<StoreItem?> GetItems();
        void RemoveItem(int id);
        void RemoveItems(IEnumerable<WastedItem?> items);
    }
}