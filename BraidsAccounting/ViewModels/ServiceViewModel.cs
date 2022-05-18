using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Models;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace BraidsAccounting.ViewModels;

internal class ServiceViewModel : ViewModelBase
{
    private readonly Services.Interfaces.IServiceProvider serviceProvider;
    private readonly IViewService viewService;
    private const int stockWarningThreshold = 2;
    private Employee? selectedEmployee;

    public ServiceViewModel(
        Services.Interfaces.IServiceProvider serviceProvider
        , IViewService viewService
        )
    {
        this.serviceProvider = serviceProvider;
        this.viewService = viewService;
        Notifier.SetMessageParams(MessageType.Warning, new(false));
        Title = "Услуги";
    }

    /// <summary>
    /// Выполненная работа, заполненная в представлении
    /// </summary>
    public Service Service { get; set; } = new();
    /// <summary>
    /// Список израсходованных материалов выполненной работы.
    /// </summary>
    public ObservableCollection<FormItem> WastedItems { get; set; } = new();
    /// <summary>
    /// Выбранный материал в представлении, который будет добавлен
    /// в список израсходованных материалов.
    /// </summary>
    public FormItem SelectedWastedItem { get; set; } = new();
    /// <summary>
    /// Список имён сотрудников, которые когда-либо выполняли работу.
    /// </summary>
    public List<Employee>? Employees { get; set; }
    /// <summary>
    /// Выбранный сотрудник, оказавший услугу.
    /// </summary>
    public Employee SelectedEmployee
    {
        get => selectedEmployee;
        set
        {
            selectedEmployee = value;
            Service.Employee = value;
        }
    }

    /// <summary>
    /// Добавляет выбранный материал в коллекцию израсходованных
    /// материалов выполненной работы
    /// </summary>
    /// <param name="storeItem"></param>
    private void AddWastedItemToService(StoreItem storeItem)
    {
        if (WastedItems.Any(wi => wi.Equals(storeItem)))
        {
            Notifier.AddError(Messages.SelectedItemAlreadyExists);
            return;
        }
        FormItem formItem = storeItem.Item;
        if (formItem.MaxCount == 0)
        {
            Notifier.AddError(Messages.SelectedItemOutOfStock);
            return;
        }
        WastedItems.Add(formItem);
    }
    private static void CheckRunningOutItems(IEnumerable<FormItem> wastedItems)
    {
        FormItem[]? runningOutItems = wastedItems.Where(i => i.MaxCount - i.Count <= stockWarningThreshold).ToArray();
        if (runningOutItems.Length > 0)
        {
            StringBuilder sb = new();
            sb.Append("Заканчиваются следующие материалы:");
            sb.Append(Environment.NewLine);
            foreach (FormItem? item in runningOutItems)
            {
                sb.Append($"{item.Manufacturer} {item.Article} {item.Color} осталось {item.MaxCount - item.Count} шт.");
                sb.Append(Environment.NewLine);
            }
            MessageBox.Show(sb.ToString(), "ВНИМАНИЕ!");
        }
    }

    #region Command CreateService - Добавление сервиса

    private DelegateCommand? _CreateServiceCommand;
    /// <summary>Команда - Добавление сервиса</summary>
    public DelegateCommand CreateServiceCommand => _CreateServiceCommand
        ??= new(OnCreateServiceCommandExecuted, CanCreateServiceCommandExecute);
    private bool CanCreateServiceCommandExecute() => true;
    private async void OnCreateServiceCommandExecuted()
    {
        MDDialogHost.CloseDialogCommand.Execute(null, null);        
        try
        {
            await serviceProvider.AddAsync(Service);
            CheckRunningOutItems(WastedItems);
            Service = new();
            WastedItems = new();
            SelectedEmployee = new();
            Notifier.AddInfo(Messages.AddServiceSuccess);
        }
        catch (Exception ex)
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
        viewService.ShowPopupWindow(nameof(SelectStoreItemView), null, (p) =>
        {
            StoreItem? item = p?[ParameterNames.SelectedItem] as StoreItem;
            if (item is not null)
            {
                AddWastedItemToService(item);
                Notifier.Remove(Messages.WastedItemNotSelected);
            }
        });

    #endregion

    #region Command OpenDialog - Команда открыть диалог

    private ICommand? _OpenDialogCommand;
    /// <summary>Команда - открыть диалог</summary>
    public ICommand OpenDialogCommand => _OpenDialogCommand
        ??= new DelegateCommand(OnOpenDialogCommandExecuted, CanOpenDialogCommandExecute);
    private bool CanOpenDialogCommandExecute() => true;
    private void OnOpenDialogCommandExecuted()
    {
        Service.WastedItems = new();
        foreach (FormItem? item in WastedItems)
            Service.WastedItems.Add(item);
        if (!serviceProvider.Validate(Service, out IEnumerable<string> errorMessages))
        {
            foreach (var errorMessage in errorMessages)
                Notifier.AddError(errorMessage);
            return;
        }        
        Notifier.Remove(Messages.WastedItemNotSelected);
        if (WastedItems.Count == 0)
            Notifier.AddWarning(Messages.WastedItemNotSelected);       
        MDDialogHost.OpenDialogCommand.Execute(null, null);
    }

    #endregion

    #region Command RemoveWastedItem - Команда удалить использованный материал

    private ICommand? _RemoveWastedItemCommand;
    /// <summary>Команда - удалить использованный материал</summary>
    public ICommand RemoveWastedItemCommand => _RemoveWastedItemCommand
        ??= new DelegateCommand(OnRemoveWastedItemCommandExecuted, CanRemoveWastedItemCommandExecute);
    private bool CanRemoveWastedItemCommandExecute() => true;
    private void OnRemoveWastedItemCommandExecuted() => WastedItems.Remove(SelectedWastedItem);

    #endregion

    #region Command InitializeData - Команда заполнить форму начальными данными

    private ICommand? _InitializeDataCommand;

    /// <summary>Команда - заполнить форму начальными данными</summary>
    public ICommand InitializeDataCommand => _InitializeDataCommand
        ??= new DelegateCommand(OnInitialDataCommandExecuted, CanInitialDataCommandExecute);
    private bool CanInitialDataCommandExecute() => true;
    private async void OnInitialDataCommandExecuted()
    {
        IEmployeesService? employeesService = ServiceLocator.GetService<IEmployeesService>();
        Employees = new(await employeesService.GetAllAsync());
    }

    #endregion
}

