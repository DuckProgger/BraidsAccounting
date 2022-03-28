using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    /// <summary>
    /// Позиция на складе.
    /// </summary>
    public class StoreItem : Entity
    {
        public int ItemId { get; set; }
        /// <summary>
        /// Материал.
        /// </summary>
        public Item Item { get; set; } = null!;
        /// <summary>
        /// Количество.
        /// </summary>
        public int Count { get; set; }
    }
}
