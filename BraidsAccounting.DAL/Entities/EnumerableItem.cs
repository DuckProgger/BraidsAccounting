using BraidsAccounting.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    [Owned]
    public class EnumerableItem : IEquatable<EnumerableItem>
    {
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool Equals(EnumerableItem? other)
        {
            if (other is null) return false;
            return Item.Equals(other.Item) && Price == other.Price;
        }
    }
}
