using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    /// <summary>
    /// Данные производителя материала.
    /// </summary>
    public class Manufacturer : Entity, IEquatable<Manufacturer>
    {
        /// <summary>
        /// Название производителя.
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Стоимость материалов данного производителя.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public List<Item> Items { get; set; } = null!;

        public bool Equals(Manufacturer? other)
        {
            if (other is null || other.Name is null) throw new ArgumentNullException(nameof(other));
            return Name.ToUpper() == other.Name.ToUpper();
        }

        //public override bool Equals(object? obj) => Equals(obj as Manufacturer);
    }
}
