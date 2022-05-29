namespace BraidsAccounting;

public static class Resources
{
    private static string GetResourceString(string key) => (string)System.Windows.Application.Current.Resources[key];

    #region Item
    internal static string LoadingItems => GetResourceString(nameof(LoadingItems));
    internal static string EditItemSuccess => GetResourceString(nameof(EditItemSuccess));
    internal static string AddItemSuccess => GetResourceString(nameof(AddItemSuccess));
    internal static string RemoveItemSuccess => GetResourceString(nameof(RemoveItemSuccess));
    internal static string ItemUsedInStore => GetResourceString(nameof(ItemUsedInStore));
    internal static string ItemUsedInService => GetResourceString(nameof(ItemUsedInService));
    internal static string ArticleNotFilled => GetResourceString(nameof(ArticleNotFilled));
    internal static string ColorNotFilled => GetResourceString(nameof(ColorNotFilled));
    internal static string ManufacturerNotFilled => GetResourceString(nameof(ManufacturerNotFilled));
    internal static string DublicateItem => GetResourceString(nameof(DublicateItem));
    #endregion

    #region StoreItem
    internal static string LoadingStoreItems => GetResourceString(nameof(LoadingStoreItems));
    internal static string RemoveStoreItemSuccess => GetResourceString(nameof(RemoveStoreItemSuccess));
    internal static string AddStoreItemSuccess => GetResourceString(nameof(AddStoreItemSuccess));
    internal static string EditStoreItemSuccess => GetResourceString(nameof(EditStoreItemSuccess));
    internal static string InvalidStoreItemCount => GetResourceString(nameof(InvalidStoreItemCount));
    internal static string StoreItemNotFound => GetResourceString(nameof(StoreItemNotFound));
    internal static string StoreItemOutOfStock => GetResourceString(nameof(StoreItemOutOfStock));
    #endregion

    #region Employee
    internal static string AddEmployeeSuccess => GetResourceString(nameof(AddEmployeeSuccess));
    internal static string EditEmployeeSuccess => GetResourceString(nameof(EditEmployeeSuccess));
    internal static string EmployeeNotSelected => GetResourceString(nameof(EmployeeNotSelected));
    internal static string EmployeeNameNotFilled => GetResourceString(nameof(EmployeeNameNotFilled));
    internal static string EmployeeNotFound => GetResourceString(nameof(EmployeeNotFound));
    internal static string DublicateEmployee => GetResourceString(nameof(DublicateEmployee));
    #endregion

    #region Manufacturer
    internal static string AddManufacturerSuccess => GetResourceString(nameof(AddManufacturerSuccess));
    internal static string EditManufacturerSuccess => GetResourceString(nameof(EditManufacturerSuccess));
    internal static string ManufacturerNotFound => GetResourceString(nameof(ManufacturerNotFound));
    internal static string ManufacturerUsedInCatalogue => GetResourceString(nameof(ManufacturerUsedInCatalogue));
    internal static string RemoveManufacturerSuccess => GetResourceString(nameof(RemoveManufacturerSuccess));
    internal static string InvalidManufacturerPrice => GetResourceString(nameof(InvalidManufacturerPrice));
    internal static string ManufacturerNameNotFilled => GetResourceString(nameof(ManufacturerNameNotFilled));
    internal static string DublicateManufacturer => GetResourceString(nameof(DublicateManufacturer));
    #endregion

    #region Payment
    internal static string AddPaymentSuccess => GetResourceString(nameof(AddPaymentSuccess));
    internal static string AmountMustBePositive => GetResourceString(nameof(AmountMustBePositive));
    #endregion

    #region Service
    internal static string SelectedItemAlreadyExists => GetResourceString(nameof(SelectedItemAlreadyExists));
    internal static string SelectedItemOutOfStock => GetResourceString(nameof(SelectedItemOutOfStock));
    internal static string AddServiceSuccess => GetResourceString(nameof(AddServiceSuccess));
    internal static string WastedItemNotSelected => GetResourceString(nameof(WastedItemNotSelected));
    internal static string WastedItemInvalidCount => GetResourceString(nameof(WastedItemInvalidCount));
    internal static string InvalidServiceProfit => GetResourceString(nameof(InvalidServiceProfit));
    #endregion

    #region SelectItem
    internal static string ItemNotSelected => GetResourceString(nameof(ItemNotSelected));
    #endregion

}
