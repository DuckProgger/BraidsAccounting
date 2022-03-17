using System.ComponentModel.DataAnnotations;

namespace BraidsAccounting.DAL.Entities.Base
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
    }

    public abstract class NamedEntity
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
