using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities;

/// <summary>
/// Выполненная работа.
/// </summary>
[Description("Услуга")]
public class Service : Entity
{
    /// <summary>
    /// Выручка.
    /// </summary>

    [Column(TypeName = "decimal(18,2)")]
    [Description("Стоимость работ")]
    public decimal Profit { get; set; }

    /// <summary>
    /// Чистая прибыль с учётом стоимости израсходованных материалов.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Description("Чистая прибыль")]
    public decimal NetProfit { get; set; }
    /// <summary>
    /// Сотрудник, выполнивший работу.
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    public Employee Employee { get; set; } = null!;
    /// <summary>
    /// Дата выполнения работы.
    /// </summary>
    [Description("Время и дата")]    
    public DateTime DateTime { get; set; }

    public List<WastedItem> WastedItems { get; set; } = null!;
}
