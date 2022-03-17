using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Models
{
    public class WarehouseItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int Count { get; set; }
    }
}
