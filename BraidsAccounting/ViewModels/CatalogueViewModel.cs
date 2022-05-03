using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels;

internal class CatalogueViewModel : ViewModelBase<Item>
{
    private ICommand? _NavigateToOtherWindowCommand;
    private readonly IViewService viewService;
    private ICatalogueService catalogueService;

    public CatalogueViewModel(
        IViewService viewService
        , ICatalogueService catalogueService
        )
    {
        this.viewService = viewService;
        this.catalogueService = catalogueService;
    }
    public string Title => "Каталог";

    public Item SelectedItem { get; set; }

    /// <summary>
    /// Флаг фильтрации отображаемых элементов каталога
    /// материалов - только в наличии.
    /// </summary>
    public bool OnlyInStock { get; set; } 

    #region Command LoadData - Команда загрузки данных со склада

    private ICommand? _LoadDataCommand;
    /// <summary>Команда - загрузки данных со склада</summary>
    public ICommand LoadDataCommand => _LoadDataCommand
        ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
    private bool CanLoadDataCommandExecute() => true;
    private async void OnLoadDataCommandExecuted()
    {
        Notifier.AddInfo(MessageContainer.LoadingItems);
        // Нужно обновить контекст, чтобы получать обновлённые данные
        catalogueService = ServiceLocator.GetService<ICatalogueService>();
        Collection = new(await catalogueService.GetAllAsync(OnlyInStock));
        Notifier.Remove(MessageContainer.LoadingItems);
    }

    #endregion

    #region Command NavigateToOtherWindow - Команда перейти на другой экран       

    /// <summary>Команда - перейти на другой экран</summary>
    public ICommand NavigateToOtherWindowCommand => _NavigateToOtherWindowCommand
        ??= new DelegateCommand<string>(OnNavigateToOtherWindowCommandExecuted, CanNavigateToOtherWindowCommandExecute);

    private bool CanNavigateToOtherWindowCommandExecute(string viewName) => true;
    private void OnNavigateToOtherWindowCommandExecuted(string viewName)
    {
        NavigationParameters? parameters = new NavigationParameters();
        if (SelectedItem is not null)
            parameters.Add("item", SelectedItem);
        viewService.ShowPopupWindow(viewName, parameters, (p) =>
        {
            if (p is not null) ResultNotifying(p);
            LoadDataCommand.Execute(null);
        });
    }

    #endregion

    #region Command RemoveItem - Команда удалить материал из каталога

    private ICommand? _RemoveItemCommand;
    /// <summary>Команда - удалить материал из каталога</summary>
    public ICommand RemoveItemCommand => _RemoveItemCommand
        ??= new DelegateCommand(OnRemoveItemCommandExecuted, CanRemoveItemCommandExecute);
    private bool CanRemoveItemCommandExecute() => true;
    private async void OnRemoveItemCommandExecuted()
    {
        await catalogueService.RemoveAsync(SelectedItem);
        Collection.Remove(SelectedItem);
        Notifier.AddWarning(MessageContainer.RemoveItemSuccess);
        MDDialogHost.CloseDialogCommand.Execute(null, null);
    }

    #endregion

    private void ResultNotifying(NavigationParameters parameters)
    {
        bool result;
        if (parameters.ContainsKey(ParameterNames.AddItemResult))
        {
            result = (bool)parameters[ParameterNames.AddItemResult];
            if (result) Notifier.AddInfo(MessageContainer.AddItemSuccess);
            return;
        }
        if (parameters.ContainsKey(ParameterNames.EditItemResult))
        {
            result = (bool)parameters[ParameterNames.EditItemResult];
            if (result) Notifier.AddInfo(MessageContainer.EditItemSuccess);
            return;
        }
    }

}
