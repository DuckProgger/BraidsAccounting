using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.Models
{
    public class Item : EntityBase
    {
        public string Manufacturer { get; set; } = null!;
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Price { get; set; }
        public WarehouseItem WarehouseItem { get; set; } = null!;
    }
}
