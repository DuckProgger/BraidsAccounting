using BraidsAccounting.Models;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ObservableCollection<WastedItemForm>? WastedItems { get; set; }
        public List<string>? Names { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public StatisticsFilterOptions FilterOptions { get; set; } = new();
        public decimal TotalExpenses { get; set; }

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
            WastedItems = new(wastedItemsService.GetWastedItemForms(FilterOptions));
            TotalExpenses = 0;
            foreach (var wastedItem in WastedItems)
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
            Names = new(serviceProvider.GetNames());
            InitializeDatePeriod();
        }

        #endregion

        #region Command ResetFilters - Команда сбросить фильтры

        private ICommand? _ResetFiltersCommand;
        /// <summary>Команда - сбросить фильтры</summary>
        public ICommand ResetFiltersCommand => _ResetFiltersCommand
            ??= new DelegateCommand(OnResetFilterCommandExecuted, CanResetFilterCommandExecute);
        private bool CanResetFilterCommandExecute() => true;
        private async void OnResetFilterCommandExecuted()
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
        private async void OnEnableWorkerFilterCommandExecuted()
        {
            if (!string.IsNullOrEmpty(FilterOptions.WorkerNameFilter))
                FilterOptions.EnableWorkerFilter = true;
        }

        #endregion
    }
}
