using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels;

internal class StoreViewModel : ViewModelBase<StoreItem>
{
    private IStoreService store;
    private readonly IEventAggregator eventAggregator;
    private readonly IContainerProvider container;
    private readonly IRegionManager regionManager;
    private readonly IViewService viewService;

    public StoreViewModel(
        IStoreService store
        , IEventAggregator eventAggregator
        , IContainerProvider container
        , IRegionManager regionManager
        , IViewService viewService
        )
    {
        this.store = store;
        this.eventAggregator = eventAggregator;
        this.container = container;
        this.regionManager = regionManager;
        this.viewService = viewService;
    }

    public string Title => "Склад";

    /// <summary>
    /// Выбранный материал со склада.
    /// </summary>
    public StoreItem SelectedStoreItem { get; set; } = null!;

    /// <summary>
    /// Общее количество материалов на складе.
    /// </summary>
    public int TotalItems { get; private set; }

    #region Command RemoveItem - Команда удалить предмет со склада

    private ICommand? _RemoveItemCommand;
    /// <summary>Команда - удалить предмет со склада</summary>
    public ICommand RemoveItemCommand => _RemoveItemCommand
        ??= new DelegateCommand(OnRemoveItemCommandExecuted, CanRemoveItemCommandExecute);
    private bool CanRemoveItemCommandExecute() => true;
    private async void OnRemoveItemCommandExecuted()
    {
        await store.RemoveItemAsync(SelectedStoreItem.Id);
        Collection.Remove(SelectedStoreItem);
        MDDialogHost.CloseDialogCommand.Execute(null, null);
        Notifier.AddInfo(MessageContainer.RemoveStoreItemSuccess);
    }

    #endregion

    #region Command LoadData - Команда загрузки данных со склада

    private ICommand? _LoadDataCommand;
    /// <summary>Команда - загрузки данных со склада</summary>
    public ICommand LoadDataCommand => _LoadDataCommand
        ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
    private bool CanLoadDataCommandExecute() => true;
    private async void OnLoadDataCommandExecuted() => await LoadData();

    private async Task LoadData()
    {
        Notifier.AddWarning(MessageContainer.LoadingStoreItems);
        // Нужно обновить контекст, чтобы получать обновлённые данные
        store = ServiceLocator.GetService<IStoreService>();
        Collection = new(await store.GetItemsAsync());
        Notifier.Remove(MessageContainer.LoadingStoreItems);
        TotalItems = Collection.Sum(i => i.Count);
    }

    #endregion

    #region Command NavigateToOtherWindow - Команда перейти на другой экран

    private ICommand? _NavigateToOtherWindowCommand;

    /// <summary>Команда - перейти на другой экран</summary>
    public ICommand NavigateToOtherWindowCommand => _NavigateToOtherWindowCommand
        ??= new DelegateCommand<string>(OnNavigateToOtherWindowCommandExecuted, CanNavigateToOtherWindowCommandExecute);

    private bool CanNavigateToOtherWindowCommandExecute(string viewName) => true;
    private void OnNavigateToOtherWindowCommandExecuted(string viewName)
    {
        NavigationParameters? parameters = new NavigationParameters();
        if (viewName.Equals(nameof(EditStoreItemView)))
            parameters.Add(ParameterNames.SelectedItem, SelectedStoreItem);
        viewService.ShowPopupWindow(viewName, parameters, (p) =>
        {
            if (p is not null) ResultNotifying(p);
            LoadDataCommand.Execute(null);
        });
    }

    #endregion          

    private void ResultNotifying(NavigationParameters parameters)
    {
        bool result;
        if (parameters.ContainsKey(ParameterNames.AddItemResult))
        {
            result = (bool)parameters[ParameterNames.AddItemResult];
            if (result) Notifier.AddInfo(MessageContainer.AddStoreItemSuccess);
            return;
        }
        if (parameters.ContainsKey(ParameterNames.EditItemResult))
        {
            result = (bool)parameters[ParameterNames.EditItemResult];
            if (result) Notifier.AddInfo(MessageContainer.EditStoreItemSuccess);
            return;
        }
    }
}
