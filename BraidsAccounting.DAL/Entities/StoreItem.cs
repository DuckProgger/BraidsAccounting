using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;

namespace BraidsAccounting.DAL.Entities;

/// <summary>
/// Позиция на складе.
/// </summary>
[Description("Материал на складе")]
public class StoreItem : Entity
{
    public int ItemId { get; set; }
    /// <summary>
    /// Материал.
    /// </summary>
    public Item Item { get; set; } = null!;
    /// <summary>
    /// Количество.
    /// </summary>
    [Description("Количество")]    
    public int Count { get; set; }
}
