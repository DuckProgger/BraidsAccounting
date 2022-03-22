using BraidsAccounting.DAL.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraidsAccounting.Infrastructure.Events
{
    internal class EditStoreItemEvent: PubSubEvent<StoreItem>
    {
    }
}
