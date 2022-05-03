using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class AddStoreItemViewModel : ViewModelBase
{
    private ICommand? _AddStoreItemCommand;
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
       storeItem.Count > 0 &&
       !string.IsNullOrEmpty(storeItem.Item?.Manufacturer?.Name) &&
       !string.IsNullOrEmpty(storeItem.Item?.Article) &&
       !string.IsNullOrEmpty(storeItem.Item?.Color);

    #region Command AddStoreItem - Команда добавить товар на склад

    /// <summary>Команда - добавить товар на склад</summary>
    public ICommand AddStoreItemCommand => _AddStoreItemCommand
        ??= new DelegateCommand(OnAddStoreItemCommandExecuted, CanAddStoreItemCommandExecute);
    private bool CanAddStoreItemCommandExecute() => IsValidStoreItem(StoreItem);
    private async void OnAddStoreItemCommandExecuted()
    {
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
