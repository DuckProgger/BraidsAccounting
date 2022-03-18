using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    public class Service : Entity
    {

        [Column(TypeName = "decimal(18,2)")]
        public decimal Profit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetProfit { get; set; }
        public List<EnumerableItems> Items { get; set; } = null!;

    }
}
