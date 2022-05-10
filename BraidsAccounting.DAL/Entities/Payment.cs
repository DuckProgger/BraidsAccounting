using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;

namespace BraidsAccounting.DAL.Entities;

[Description("Платёж")]
public class Payment : Entity
{
    [Description("Сумма")]
    public decimal Amount { get; set; }
    [Description("Дата и время")]
    public DateTime DateTime { get; set; }
    public Employee Employee { get; set; } = null!;
}
