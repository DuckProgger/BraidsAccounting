using BraidsAccounting.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    [Owned]
    public class EnumerableItems 
    {
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
