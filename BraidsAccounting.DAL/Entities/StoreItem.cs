﻿using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    public class StoreItem : Entity
    {
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
    }
}