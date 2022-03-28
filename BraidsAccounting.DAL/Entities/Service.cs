﻿using BraidsAccounting.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraidsAccounting.DAL.Entities
{
    /// <summary>
    /// Выполненная работа.
    /// </summary>
    public class Service : Entity
    {
        /// <summary>
        /// Выручка.
        /// </summary>

        [Column(TypeName = "decimal(18,2)")]
        public decimal Profit { get; set; }

        /// <summary>
        /// Чистая прибыль с учётом стоимости израсходованных материалов.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetProfit { get; set; }
        /// <summary>
        /// Имя сотрудника, выполнившего работы.
        /// </summary>
        public string WorkerName { get; set; } = null!;
        /// <summary>
        /// Дата выполнения работы.
        /// </summary>
        public DateTime DateTime { get; set; }

        public List<WastedItem> WastedItems { get; set; } = null!;
    }
}
