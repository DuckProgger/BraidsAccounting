using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;
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
        Title = "Каталог материалов";
    }

    public Item SelectedItem { get; set; }

    /// <summary>
    /// Флаг фильтрации отображаемых элементов каталога
    /// материалов - только в наличии.
    /// </summary>
    public bool OnlyInStock { get; set; }

    private void ResultNotifying(NavigationParameters parameters)
    {
        bool result;
        if (parameters.ContainsKey(ParameterNames.AddItemResult))
        {
            result = (bool)parameters[ParameterNames.AddItemResult];
            if (result) Notifier.AddInfo(Messages.AddItemSuccess);
            return;
        }
        if (parameters.ContainsKey(ParameterNames.EditItemResult))
        {
            result = (bool)parameters[ParameterNames.EditItemResult];
            if (result) Notifier.AddInfo(Messages.EditItemSuccess);
            return;
        }
    }    

    #region Command LoadData - Команда загрузки данных со склада

    private ICommand? _LoadDataCommand;
    /// <summary>Команда - загрузки данных со склада</summary>
    public ICommand LoadDataCommand => _LoadDataCommand
        ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
    private bool CanLoadDataCommandExecute() => true;
    private async void OnLoadDataCommandExecuted()
    {
        Notifier.AddInfo(Messages.LoadingItems);
        // Нужно обновить контекст, чтобы получать обновлённые данные
        catalogueService = ServiceLocator.GetService<ICatalogueService>();
        Collection = new(await catalogueService.GetAllAsync(OnlyInStock));
        Notifier.Remove(Messages.LoadingItems);
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
            parameters.Add(ParameterNames.SelectedItem, SelectedItem);
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
        try
        {
            await catalogueService.RemoveAsync(SelectedItem);
            Collection.Remove(SelectedItem);
            Notifier.AddInfo(Messages.RemoveItemSuccess);
        }
        catch (ArgumentException ex)
        {
            Notifier.AddError(ex.Message);
        }
        MDDialogHost.CloseDialogCommand.Execute(null, null);
    }

    #endregion

    #region Command SelectItem - Команда выбрать материал из каталога

    private ICommand? _SelectItemCommand;
    /// <summary>Команда - выбрать материал из каталога</summary>
    public ICommand SelectItemCommand => _SelectItemCommand
        ??= new DelegateCommand(OnSelectItemCommandExecuted, CanSelectItemCommandExecute);
    private bool CanSelectItemCommandExecute() => true;
    private void OnSelectItemCommandExecuted()
    {
        viewService.AddParameter(ParameterNames.SelectedItem, SelectedItem);
        viewService.GoBack();
    }

    #endregion

}
