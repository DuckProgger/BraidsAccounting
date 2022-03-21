using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class SelectStoreItemViewModel : BindableBase
    {
        public ObservableCollection<StoreItem> StoreItems { get; set; } = new();
        public StoreItem SelectedItem { get; set; }
        private readonly IEventAggregator eventAggregator;
        private readonly IStoreService store;

        public SelectStoreItemViewModel(
            IEventAggregator eventAggregator
            , IStoreService store
            )
        {
            this.eventAggregator = eventAggregator;
            this.store = store;
        }


        #region Command Select - Команда выбрать товар

        private ICommand? _SelectCommand;

        /// <summary>Команда - выбрать товар</summary>
        public ICommand SelectCommand => _SelectCommand
            ??= new DelegateCommand(OnSelectCommandExecuted, CanSelectCommandExecute);
        private bool CanSelectCommandExecute() => true;
        private async void OnSelectCommandExecuted()
        {
            eventAggregator.GetEvent<PubSubEvent<StoreItem>>().Publish(SelectedItem);
            
        }

        #endregion

        #region Command LoadData - Команда загрузки данных со склада

        private ICommand? _LoadDataCommand;
        /// <summary>Команда - загрузки данных со склада</summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
        private bool CanLoadDataCommandExecute() => true;
        private async void OnLoadDataCommandExecuted()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            StoreItems = new(store.GetItems());
        }

        #endregion
    }
}
