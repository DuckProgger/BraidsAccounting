using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    public class Manufacturer : Entity, IEquatable<Manufacturer>
    {
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public List<Item> Items { get; set; } = null!;

        public bool Equals(Manufacturer? other)
        {
            if (other is null) return false;
            return Name.ToUpper() == other.Name.ToUpper()
                //&& Price == other.Price
                ;
        }

    }
}
