using BraidsAccounting.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Entities
{
    internal class Service : Entity
    {
        public List<Supply> Supplies { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Profit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetProfit { get; set; }


    }
}
