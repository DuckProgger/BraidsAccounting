using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Infrastructure.Constants;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows.Input;
using BraidsAccounting.DAL.Exceptions;

namespace BraidsAccounting.ViewModels;

internal class EditStoreItemViewModel : ViewModelBase
{
    private readonly IStoreService store;
    private readonly IManufacturersService manufacturersService;
    private readonly IViewService viewService;
    private Manufacturer selectedManufacturer;

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

    public Manufacturer SelectedManufacturer
    {
        get => selectedManufacturer;
        set => selectedManufacturer = value;
    }

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
        Title = "Изменение материала на складе";
    }

    public async override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
        StoreItem? item = navigationContext.Parameters[ParameterNames.SelectedItem] as StoreItem;
        if (item is not null)
        {
            StoreItem = item;
            SelectedManufacturer = Manufacturers.Find(m => m.Name.Equals(item.Item.Manufacturer.Name));
        }
    }  

    #region Command SaveChanges - Команда сохранить изменения товара со склада

    private ICommand? _SaveChangesCommand;

    /// <summary>Команда - сохранить изменения товара со склада</summary>
    public ICommand SaveChangesCommand => _SaveChangesCommand
        ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);

    private bool CanSaveChangesCommandExecute() => true;
    private async void OnSaveChangesCommandExecuted()
    {
        StoreItem.Item.Manufacturer = SelectedManufacturer;
        if (!store.Validate(StoreItem, out IEnumerable<string> errorMessages))
        {
            foreach (var errorMessage in errorMessages)
                Notifier.AddError(errorMessage);
            return;
        }
        try
        {
            await store.EditAsync(StoreItem);
            viewService.AddParameter(ParameterNames.EditItemResult, true);
            viewService.GoBack();
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }
    }

    #endregion
}
