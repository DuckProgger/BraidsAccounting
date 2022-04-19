using BraidsAccounting.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Entities
{
    internal class Payment : Entity
    {
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
