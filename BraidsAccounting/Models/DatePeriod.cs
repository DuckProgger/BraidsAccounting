using System;

namespace BraidsAccounting.Models
{
    /// <summary>
    /// Представляет временной интервал.
    /// </summary>
    internal struct DatePeriod
    {
        /// <summary>
        /// Начало интервала.
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// Конец интервала.
        /// </summary>
        public DateTime End { get; set; }
    }
}
