using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities;

/// <summary>
///  Материал, используемый для выполнения работ.
/// </summary>
[Description("Материал из каталога")]
public record Item : Entity, IEquatable<Item>
{
    /// <summary>
    /// Артикул.
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    [Description("Модель")]
    public string Article { get; set; } = null!;
    /// <summary>
    /// Цвет.
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    [Description("Цвет")]
    public string Color { get; set; } = null!;
    /// <summary>
    /// Производитель.
    /// </summary>
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;
    public List<StoreItem> StoreItems { get; set; } = null!;

    public virtual bool Equals(Item? other)
    {
        //if (other is null || other.Article is null || other.Color is null) throw new ArgumentNullException(nameof(other));
        if (other is null || other.Article is null 
            || other.Color is null || Manufacturer is null
            || Article is null || Color is null) return false;
        return Article.ToUpper() == other.Article.ToUpper()
          && Color.ToUpper() == other.Color.ToUpper()
          && Manufacturer.Equals(other.Manufacturer);
    }

    //public override bool Equals(object? obj) => Equals(obj as Item);

    public override int GetHashCode() =>
        HashCode.Combine(Article, Color, Manufacturer.Id);
}
