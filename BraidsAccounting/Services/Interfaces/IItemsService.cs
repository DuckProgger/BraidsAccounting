namespace BraidsAccounting.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий сервис взаимодействия с материалами. 
    /// </summary>
    internal interface IItemsService
    {
        /// <summary>
        /// Определяет содержится ли выбранный производитель в каталоге.
        /// </summary>
        /// <param name="manufacturerName">Название производителя.</param>
        /// <returns></returns>
        bool ContainsManufacturer(string manufacturerName);
    }
}