using BraidsAccounting.Models;
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
    internal class WastedItemsViewModel : BindableBase
    {
        private readonly IWastedItemsService wastedItemsService;
        private readonly Services.Interfaces.IServiceProvider serviceProvider;

        public WastedItemsViewModel(
            IWastedItemsService wastedItemsService
            , IServiceProvider serviceProvider
            )
        {
            this.wastedItemsService = wastedItemsService;
            this.serviceProvider = serviceProvider;
        }

        public ObservableCollection<WastedItemForm> WastedItems { get; set; }
        public List<string> Names { get; set; }

        /// <summary>
        /// Флаг группировки потраченных материалов
        /// </summary>
        public bool GroupByItems { get; set; }

        /// <summary>
        /// Выбранное в форме имя сотрудника
        /// </summary>
        public string SelectedName { get; set; }
        //public DatePeriod DatePeriod { get; set; } 
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        private void InitializeDatePeriod()
        {
            //DatePeriod = new()
            //{
            Start = DateTime.Now;
            End = DateTime.Now;
            //};
        }

        #region Command GetData - Команда получить данные

        private ICommand? _GetDataCommand;
        /// <summary>Команда - получить данные</summary>
        public ICommand GetDataCommand => _GetDataCommand
            ??= new DelegateCommand(OnGetDataCommandExecuted, CanGetDataCommandExecute);
        private bool CanGetDataCommandExecute() => true;
        private async void OnGetDataCommandExecuted()
        {
            DatePeriod datePeriod = new()
            {
                Start = Start,
                End = End
            };
            WastedItems = new(wastedItemsService.GetWastedItemForms(SelectedName, GroupByItems, datePeriod));
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
            SelectedName = string.Empty;
            InitializeDatePeriod();
        }

        #endregion
    }
}
