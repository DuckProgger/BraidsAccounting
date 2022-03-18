using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    public class StoreItem : Entity
    {
        //public int ItemId { get; set; }
        public EnumerableItem EnumerableItem { get; set; } = null!;

        //public List<Service> Services { get; set; }
    }
}
