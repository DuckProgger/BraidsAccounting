using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities;

public class Payment : Entity
{
    public decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
    public Employee Employee { get; set; } = null!;
}
