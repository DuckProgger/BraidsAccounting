using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using System.Collections.Generic;
using BraidsAccounting.Services;
using BraidsAccounting.DAL.Exceptions;

namespace BraidsAccounting.ViewModels;

internal class AddStoreItemViewModel : ViewModelBase
{
    private readonly IStoreService store;
    private readonly IManufacturersService manufacturersService;
    private readonly IViewService viewService;
    private bool newItem;

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
    public Manufacturer? SelectedManufacturer { get; set; }

    public int InStock { get; set; }

    public string Article { get; set; }
    public string Color { get; set; }
    public int Count { get; set; }

    public bool NewItem
    {
        get => newItem;
        set
        {
            newItem = value;
            Article = string.Empty;
            Color = string.Empty;
            Count = 0;
            InStock = 0;
            SelectedManufacturer = null;
        }
    }

    public AddStoreItemViewModel(IStoreService store
        , IManufacturersService manufacturersService
        , IViewService viewService)
    {
        this.store = store;
        this.manufacturersService = manufacturersService;
        this.viewService = viewService;
        Title = "Добавление материала на склад";
        StoreItem = new() { Item = new() };
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Item? item = navigationContext.Parameters[ParameterNames.SelectedItem] as Item;
        if (item is not null)
        {
            Article = item.Article;
            Color = item.Color;
            SelectedManufacturer = Manufacturers.Find(m => m.Name.Equals(item.Manufacturer.Name));
            InStock = store.Count(item.Manufacturer.Name, item.Article, item.Color);
        }
    }

    private static bool IsValidStoreItem(StoreItem storeItem) =>
       IsValidCount(storeItem.Count) &&
       !string.IsNullOrEmpty(storeItem.Item?.Manufacturer?.Name) &&
       !string.IsNullOrEmpty(storeItem.Item?.Article) &&
       !string.IsNullOrEmpty(storeItem.Item?.Color);

    private static bool IsValidCount(int count) => count > 0;

    #region Command AddStoreItem - Команда добавить товар на склад

    private DelegateCommand? _AddStoreItemCommand;

    /// <summary>Команда - добавить товар на склад</summary>
    public DelegateCommand AddStoreItemCommand => _AddStoreItemCommand
        ??= new DelegateCommand(OnAddStoreItemCommandExecuted, CanAddStoreItemCommandExecute);

    private bool CanAddStoreItemCommandExecute() => true;
    private async void OnAddStoreItemCommandExecuted()
    {
        StoreItem = new() { Item = new() };
        StoreItem.Item.Color = Color;
        StoreItem.Item.Article = Article;
        StoreItem.Item.Manufacturer = SelectedManufacturer;
        StoreItem.Count = Count;
        if (!IsValidCount(StoreItem.Count))
        {
            Notifier.AddError(Messages.StoreItemInvalidCount);
            return;
        }
        if (!IsValidStoreItem(StoreItem))
        {
            Notifier.AddError(Messages.FieldsNotFilled);
            return;
        }
        try
        {
            await store.AddAsync(StoreItem);
            viewService.AddParameter(ParameterNames.AddItemResult, true);
            viewService.GoBack();
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }
       
    }

    #endregion

    #region Command SelectStoreItem - Команда выбрать товар со склада

    private ICommand? _SelectStoreItemCommand;
    /// <summary>Команда - выбрать товар со склада</summary>
    public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
        ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
    private bool CanSelectStoreItemCommandExecute() => true;
    private void OnSelectStoreItemCommandExecuted() =>
        viewService.ShowPopupWindow(nameof(SelectItemView));

    #endregion

    #region Command GetManufacturers - Команда получить список производителей

    private ICommand? _GetManufacturersCommand;
    /// <summary>Команда - получить список производителей</summary>
    public ICommand GetManufacturersCommand => _GetManufacturersCommand
        ??= new DelegateCommand(OnGetManufacturersCommandExecuted, CanGetManufacturersCommandExecute);
    private bool CanGetManufacturersCommandExecute() => true;
    private async void OnGetManufacturersCommandExecuted()
    {
        IManufacturersService? manufacturersService = ServiceLocator.GetService<IManufacturersService>();
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
    }

    #endregion

}
