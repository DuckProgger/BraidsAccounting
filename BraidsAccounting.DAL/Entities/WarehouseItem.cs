using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.Models
{
    public class WarehouseItem : EntityBase
    {
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
    }
}
