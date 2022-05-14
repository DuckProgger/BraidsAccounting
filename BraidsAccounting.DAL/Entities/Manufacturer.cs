using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities;
/// <summary>
/// Данные производителя материала.
/// </summary>
[Description("Производитель")]
public record Manufacturer : Entity, IEquatable<Manufacturer>
{
    /// <summary>
    /// Название производителя.
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    [Description("Имя")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Стоимость материалов данного производителя.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Description("Цена")]
    public decimal Price { get; set; }
    public List<Item> Items { get; set; } = null!;

    public virtual bool Equals(Manufacturer? other)
    {
        //if (other is null || other.Name is null) throw new ArgumentNullException(nameof(other));
        if (other is null || other.Name is null || Name is null) return false;
        return Name.ToUpper() == other.Name.ToUpper();
    }

    //public override bool Equals(object? obj) => Equals(obj as Manufacturer);
}
