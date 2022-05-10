using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel;

namespace BraidsAccounting.DAL.Entities;

[Description("Сотрудник")]
public record Employee : Entity
{
    [Description("Имя")]
    public string Name { get; set; } = null!;
}
