using BraidsAccounting.DAL.Entities.Base;

namespace BraidsAccounting.DAL.Entities;

public class Employee : Entity
{
    public string Name { get; set; } = null!;
}
