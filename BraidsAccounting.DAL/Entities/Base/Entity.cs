namespace BraidsAccounting.DAL.Entities.Base;

public abstract record Entity : IEntity
{
    public int Id { get; set; }
}   
