using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Models
{
    internal class WastedItemForm
    {
        public string? ItemName { get; set; }
        public string? Article { get; set; }
        public string? Color { get; set; }
        public int Count { get; set; }
        public decimal Expense { get; set; }
    }
}
