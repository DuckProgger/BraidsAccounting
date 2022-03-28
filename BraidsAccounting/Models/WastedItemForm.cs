namespace BraidsAccounting.Models
{
    /// <summary>
    /// Представляет класс для отображения информации об израсходованном материале в форме.
    /// </summary>
    internal class WastedItemForm
    {
        /// <summary>
        /// Производитель материала.
        /// </summary>
        public string? Manufacturer { get; set; }
        /// <summary>
        /// Артикул материала.
        /// </summary>
        public string? Article { get; set; }
        /// <summary>
        /// Цвет материала.
        /// </summary>
        public string? Color { get; set; }
        /// <summary>
        /// Количество материалов.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Расходы на материалы.
        /// </summary>
        public decimal Expense { get; set; }
    }
}
