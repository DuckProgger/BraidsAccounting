using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities
{
    public class WastedItem : Entity
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
    }
}
