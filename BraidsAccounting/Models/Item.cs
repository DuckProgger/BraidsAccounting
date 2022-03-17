using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = null!;
        public string Article { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Price { get; set; }
        //public int WarehouseItemId { get; set; }
        public WarehouseItem WarehouseItem { get; set; } = null!;
    }
}
