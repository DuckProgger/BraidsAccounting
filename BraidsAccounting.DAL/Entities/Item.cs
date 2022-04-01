using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities
{
    /// <summary>
    ///  Материал, используемый для выполнения работ.
    /// </summary>
    public class Item : Entity, IEquatable<Item>
    {
        /// <summary>
        /// Артикул.
        /// </summary>
        public string Article { get; set; } = null!;
        /// <summary>
        /// Цвет.
        /// </summary>
        public string Color { get; set; } = null!;
        /// <summary>
        /// Производитель.
        /// </summary>
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;
        public List<StoreItem> StoreItems { get; set; } = null!;

        public bool Equals(Item? other)
        {
            if (other is null || other.Article is null || other.Color is null) throw new ArgumentNullException(nameof(other));
            return Article.ToUpper() == other.Article.ToUpper()
              && Color.ToUpper() == other.Color.ToUpper()
              && Manufacturer.Equals(other.Manufacturer);
        }

        public override bool Equals(object? obj) => Equals(obj as Item);       
    }
}
