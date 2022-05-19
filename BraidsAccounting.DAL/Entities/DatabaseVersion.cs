using BraidsAccounting.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.DAL.Entities;

public record DatabaseVersion : Entity
{
    public int Version { get; set; }
    public DateTime DateTime { get; set; }
}
