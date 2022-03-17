using BraidsAccounting.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BraidsAccounting.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }   
}
