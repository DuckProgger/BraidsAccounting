using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities
{
    public class Item : Entity
    {
        public string Manufacturer { get; set; } = null!;
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        //public Store Supply { get; set; } = null!;
    }
}
