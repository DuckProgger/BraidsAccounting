using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class AddStoreItemViewModel : ViewModelBase
{
    private readonly IStoreService store;
    private readonly IViewService viewService;

    /// <summary>
    /// Материал со склада, обрабатываемый в форме.
    /// </summary>
    public StoreItem StoreItem { get; set; } = new();

    public int InStock { get; set; }

    public AddStoreItemViewModel(IStoreService store, IViewService viewService)
    {
        this.store = store;
        this.viewService = viewService;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Item? item = navigationContext.Parameters[ParameterNames.SelectedItem] as Item;
        if (item is not null)
        {
            StoreItem = new() { Item = item };
            InStock = store.GetItemCount(item.Manufacturer.Name, item.Article, item.Color);
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
        await store.AddItemAsync(StoreItem);
        viewService.AddParameter(ParameterNames.AddItemResult, true);
        viewService.GoBack();
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

}
