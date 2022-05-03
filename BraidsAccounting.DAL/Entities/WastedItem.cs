using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities;

/// <summary>
/// Израсходованный материал.
/// </summary>
public class WastedItem : Entity
{
    public int ServiceId { get; set; }
    /// <summary>
    /// Работа, для которой материал был использован.
    /// </summary>
    public Service Service { get; set; } = null!;
    public int ItemId { get; set; }
    /// <summary>
    /// Материал, использованный для работы.
    /// </summary>
    public Item Item { get; set; } = null!;
    /// <summary>
    /// Количество материалов.
    /// </summary>
    public int Count { get; set; }
}
