using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    public class ItemPrice : Entity, IEquatable<ItemPrice>
    {
        public string Manufacturer { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public List<Item> Items { get; set; } = null!;

        public bool Equals(ItemPrice? other)
        {
            if (other is null) return false;
            return Manufacturer.ToUpper() == other.Manufacturer.ToUpper()
                //&& Price == other.Price
                ;
        }

    }
}
