using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities;

/// <summary>
/// Израсходованный материал.
/// </summary>
[Description("Израсходованный материал")]
public record WastedItem : Entity
{
    public int ServiceId { get; set; }
    /// <summary>
    /// Работа, для которой материал был использован.
    /// </summary>
    public Service Service { get; set; } = null!;
    /// <summary>
    /// Материал, использованный для работы.
    /// </summary>
    public Item Item { get; set; } = null!;
    /// <summary>
    /// Цена, по которой продали материал,
    /// взятая из актуальной цены производителя.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    /// <summary>
    /// Количество материалов.
    /// </summary>
    [Description("Количество")]
    public int Count { get; set; }
}
