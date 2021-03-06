using BraidsAccounting.Infrastructure;
using BraidsAccounting.Models;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using IServiceProvider = BraidsAccounting.Services.Interfaces.IServiceProvider;

namespace BraidsAccounting.ViewModels;

internal class WastedItemsViewModel : ViewModelBase<WastedItemForm>
{
    private readonly IWastedItemsService statisticsService;
    private readonly IServiceProvider serviceProvider;

    public WastedItemsViewModel(
        IWastedItemsService statisticsService
        , IServiceProvider serviceProvider
        )
    {
        this.statisticsService = statisticsService;
        this.serviceProvider = serviceProvider;
        Title = "Израсходованные материалы";
    }

    /// <summary>
    /// Список имён сотрудников, которые когда-либо выполняли работу.
    /// </summary>
    public List<string>? Names { get; set; }
    /// <summary>
    /// Значение, введённое в поле фильтра начала периода.
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// Значение, введённое в поле фильтра конца периода.
    /// </summary>
    public DateTime End { get; set; }
    /// <summary>
    /// Опции фильтрации.
    /// </summary>
    public FilterOptions FilterOptions { get; set; }
    /// <summary>
    /// Сумма расходов за материалы из списка израсходованных материалов.
    /// </summary>
    public decimal TotalExpenses { get; set; }

    /// <summary>
    /// Задаёт значения фильтров по умолчанию.
    /// </summary>
    private void RestoreFilterDefaults()
    {
        FilterOptions = new()
        {
            EnablePeriodFilter = true
        };
        Start = DateTime.Now.Date.AddDays(-30);
        End = DateTime.Now.Date;
    }

    #region Command GetData - Команда получить данные

    private ICommand? _GetDataCommand;
    /// <summary>Команда - получить данные</summary>
    public ICommand GetDataCommand => _GetDataCommand
        ??= new DelegateCommand(OnGetDataCommandExecuted, CanGetDataCommandExecute);
    private bool CanGetDataCommandExecute() => true;
    private async void OnGetDataCommandExecuted()
    {
        FilterOptions.DatePeriod = new()
        {
            Start = Start,
            End = End
        };
        Collection = new(await statisticsService.GetWastedItemFormsAsync(FilterOptions));
        TotalExpenses = await statisticsService.GetTotalExpensesAsync(FilterOptions);
    }

    #endregion

    #region Command InitializeData - Команда заполнить форму начальными данными

    private ICommand? _InitializeDataCommand;
    /// <summary>Команда - заполнить форму начальными данными</summary>
    public ICommand InitializeDataCommand => _InitializeDataCommand
        ??= new DelegateCommand(OnInitialDataCommandExecuted, CanInitialDataCommandExecute);
    private bool CanInitialDataCommandExecute() => true;
    private async void OnInitialDataCommandExecuted()
    {
        Names = await serviceProvider.GetNamesAsync();
        if (Collection is null || Collection.Count == 0)
            RestoreFilterDefaults();
    }

    #endregion

    #region Command ResetForm - Команда сбросить форму

    private ICommand? _ResetFormCommand;
    /// <summary>Команда - сбросить фильтры</summary>
    public ICommand ResetFormCommand => _ResetFormCommand
        ??= new DelegateCommand(OnResetFormCommandExecuted, CanResetFormCommandExecute);
    private bool CanResetFormCommandExecute() => true;
    private void OnResetFormCommandExecuted()
    {
        RestoreFilterDefaults();
        Collection = null;
        TotalExpenses = 0;
    }

    #endregion

    #region Command EnableWorkerFilter - Команда включить фильтр по сотрудникам

    private ICommand? _EnableWorkerFilterCommand;
    /// <summary>Команда - включить фильтр по сотрудникам</summary>
    public ICommand EnableWorkerFilterCommand => _EnableWorkerFilterCommand
        ??= new DelegateCommand(OnEnableWorkerFilterCommandExecuted, CanEnableWorkerFilterCommandExecute);
    private bool CanEnableWorkerFilterCommandExecute() => true;
    private void OnEnableWorkerFilterCommandExecuted()
    {
        if (!string.IsNullOrEmpty(FilterOptions.WorkerNameFilter))
            FilterOptions.EnableWorkerFilter = true;
    }

    #endregion
}
