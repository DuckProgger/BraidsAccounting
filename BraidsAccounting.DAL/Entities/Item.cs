using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities
{
    public class Item : Entity, IEquatable<Item>
    {
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;

        public bool Equals(Item? other)
        {
            if (other is null) return false;
            return Article.ToUpper() == other.Article.ToUpper()
              && Color.ToUpper() == other.Color.ToUpper()
              && Manufacturer.Equals(other.Manufacturer);
        }

    }
}
