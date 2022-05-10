using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Infrastructure.Constants;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class EditStoreItemViewModel : ViewModelBase
{
    private readonly IStoreService store;
    private readonly IManufacturersService manufacturersService;
    private readonly IViewService viewService;

    public string Title => "Изменение материала на складе";

    /// <summary>
    /// Материал со склада, обрабатываемый в форме.
    /// </summary>
    public StoreItem StoreItem { get; set; } = new();
    /// <summary>
    /// Список производителей.
    /// </summary>
    public List<Manufacturer>? Manufacturers { get; set; }
    /// <summary>
    /// Выбранный производитель из списка.
    /// </summary>

    public Manufacturer SelectedManufacturer { get; set; } = null!;
    public EditStoreItemViewModel(
     IStoreService store
        , IManufacturersService manufacturersService
        , IViewService viewService
     )
    {
        this.store = store;
        this.manufacturersService = manufacturersService;
        this.viewService = viewService;
        this.store = store;
    }

    /// <summary>
    /// Устанавливает свойства при приёме сообщения.
    /// </summary>
    /// <param name="item"></param>
    private void SetProperties(StoreItem item)
    {
        StoreItem = item;
        StoreItem.Item.Manufacturer = SelectedManufacturer;
    }
    public async override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
        StoreItem? item = navigationContext.Parameters[ParameterNames.SelectedItem] as StoreItem;
        if (item is not null)
            SetProperties(item);
    }

    private bool IsValidItem() =>
      IsValidCount(StoreItem.Count) &&
      !string.IsNullOrEmpty(SelectedManufacturer?.Name) &&
      !string.IsNullOrEmpty(StoreItem?.Item?.Color) &&
      !string.IsNullOrEmpty(StoreItem?.Item?.Article);

    private static bool IsValidCount(int count) => count > 0;

    #region Command SaveChanges - Команда сохранить изменения товара со склада

    private ICommand? _SaveChangesCommand;
    /// <summary>Команда - сохранить изменения товара со склада</summary>
    public ICommand SaveChangesCommand => _SaveChangesCommand
        ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);

    private bool CanSaveChangesCommandExecute() => true;
    private async void OnSaveChangesCommandExecuted()
    {
        StoreItem.Item.Manufacturer = SelectedManufacturer;
        if (!IsValidCount(StoreItem.Count))
        {
            Notifier.AddError(Messages.StoreItemInvalidCount);
            return;
        }
        if (!IsValidItem())
        {
            Notifier.AddError(Messages.FieldsNotFilled);
            return;
        }
        await store.EditItemAsync(StoreItem);
        viewService.AddParameter(ParameterNames.EditItemResult, true);
        viewService.GoBack();
    }

    #endregion
}
