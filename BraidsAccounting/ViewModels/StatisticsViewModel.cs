using BraidsAccounting.Models;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IServiceProvider = BraidsAccounting.Services.Interfaces.IServiceProvider;

namespace BraidsAccounting.ViewModels
{
    internal class StatisticsViewModel : BindableBase
    {
        private readonly IStatisticsService wastedItemsService;
        private readonly IServiceProvider serviceProvider;

        public StatisticsViewModel(
            IStatisticsService wastedItemsService
            , IServiceProvider serviceProvider
            )
        {
            this.wastedItemsService = wastedItemsService;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Список израсходованных материалов в представлении.
        /// </summary>
        public ObservableCollection<WastedItemForm>? WastedItems { get; set; }
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
        public StatisticsFilterOptions FilterOptions { get; set; } = new();
        /// <summary>
        /// Сумма расходов за материалы из списка израсходованных материалов.
        /// </summary>
        public decimal TotalExpenses { get; set; }
        /// <summary>
        /// Задаёт значения фильтров периода по умолчанию.
        /// </summary>
        private void InitializeDatePeriod()
        {
            Start = DateTime.Now.Date;
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
            WastedItems = new(await wastedItemsService.GetWastedItemFormsAsync(FilterOptions));
            TotalExpenses = 0;
            foreach (WastedItemForm? wastedItem in WastedItems)
                TotalExpenses += wastedItem.Expense;
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
            Names = new(await serviceProvider.GetNamesAsync());
            InitializeDatePeriod();
        }

        #endregion

        #region Command ResetFilters - Команда сбросить фильтры

        private ICommand? _ResetFiltersCommand;
        /// <summary>Команда - сбросить фильтры</summary>
        public ICommand ResetFiltersCommand => _ResetFiltersCommand
            ??= new DelegateCommand(OnResetFilterCommandExecuted, CanResetFilterCommandExecute);
        private bool CanResetFilterCommandExecute() => true;
        private void OnResetFilterCommandExecuted()
        {
            FilterOptions = new();
            InitializeDatePeriod();
            WastedItems = null;
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
}
