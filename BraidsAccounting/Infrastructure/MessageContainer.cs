namespace BraidsAccounting.Infrastructure;

internal static class MessageContainer
{
    #region Item
    public const string LoadingItems = "Загружается каталог материалов...";
    public const string AddItemSuccess = "Материал успешно добавлен в каталог.";
    public const string EditItemSuccess = "Материал успешно изменён.";
    public const string RemoveItemSuccess = "Материал успешно удалён из каталога.";
    public const string ItemUsedInStore = "Материал не может быть удалён, так как используется на складе.";
    public const string ItemUsedInService = "Материал не может быть удалён, так как используется в услуге.";
    #endregion

    #region StoreItem
    public const string LoadingStoreItems = "Загружается список материалов на складе...";
    public const string RemoveStoreItemSuccess = "Материал успешно удалён со склада.";
    public const string AddStoreItemSuccess = "Материал успешно добавлен на склад.";
    public const string EditStoreItemSuccess = "Количество материала на складе успешно изменено.";
    public const string StoreItemInvalidCount = "Количество материала должно быть больше нуля.";
    #endregion

    #region Employee
    public const string AddEmployeeSuccess = "Новый сотрудник успешно добавлен.";
    public const string EditEmployeeSuccess = "Сотрудник успешно изменён.";
    public const string EmployeeNotSelected = "Не выбран сотрудник.";
    #endregion

    #region Manufacturer
    public const string AddManufacturerSuccess = "Новый производитель успешно добавлен.";
    public const string EditManufacturerSuccess = "Производитель успешно изменён.";
    public const string ManufacturerNotFound = "Выбранного производителя нет в каталоге.";
    public const string ManufacturerUsedInCatalogue = "Производитель не может быть удалён, так как используется материалом в каталоге.";
    public const string RemoveManufacturerSuccess = "Производитель успешно удалён.";
    #endregion

    #region Payment
    public const string AddPaymentSuccess = "Сумма успешно зачислена.";
    public const string AmountMustBePositive = "Сумма должна быть положительной.";
    #endregion

    #region Service
    public const string SelectedItemAlreadyExists = "Выбранный материал уже есть в списке.";
    public const string SelectedItemOutOfStock = "Выбранный материал отсутсвует на складе.";
    public const string AddServiceSuccess = "Услуга успешно добавлена.";
    public const string WastedItemNotSelected = "НЕ ВЫБРАН НИ ОДИН МАТЕРИАЛ!";
    public const string WastedItemInvalidCount = "Количество израходованного материала должно быть больше нуля.";
    public const string InvalidServiceProfit = "Сумма прибыли должна быть положительной.";
    #endregion

    #region SelectItem
    public const string ItemNotSelected = "Не выбран ни один товар.";
    #endregion

    public const string FieldsNotFilled = "Не все поля заполнены.";

}

