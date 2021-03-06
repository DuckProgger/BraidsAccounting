using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities;

public record History : Entity
{
    public string Operation { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public string Message { get; set; } = null!;
    /// <summary>
    /// Временная отметка.
    /// </summary>
    public DateTime TimeStamp { get; set; }
}
