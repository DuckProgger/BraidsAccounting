using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class StoreViewModel : BindableBase
    {
        private readonly IStoreService store;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerProvider container;

        public StoreItem? StoreItem { get; set; }
        public ObservableCollection<StoreItem?> StoreItems { get; set; } = new();

        public StoreViewModel(
            IStoreService store
            , IEventAggregator eventAggregator
            , IContainerProvider container

            )
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
            this.container = container;
        }

        //public StoreViewModel() { }

        #region Command AddItem - Команда добавить предмет на склад

        private ICommand? _AddItemCommand;
        /// <summary>Команда - добавить предмет на склад</summary>
        public ICommand AddItemCommand => _AddItemCommand
            ??= new DelegateCommand(OnAddItemCommandExecuted, CanAddItemCommandExecute);
        private bool CanAddItemCommandExecute() => true;
        private async void OnAddItemCommandExecuted()
        {
            //store.AddItem(StoreItem);
            new AddStoreItemWindow().Show();
        }

        #endregion

        #region Command EditItem - Команда редактировать предмет на складе

        private ICommand? _EditItemCommand;
        /// <summary>Команда - редактировать предмет на складе</summary>
        public ICommand EditItemCommand => _EditItemCommand
            ??= new DelegateCommand(OnEditItemCommandExecuted, CanEditItemCommandExecute);
        private bool CanEditItemCommandExecute() => true;
        private async void OnEditItemCommandExecuted()
        {
            eventAggregator.GetEvent<PubSubEvent<StoreItem>>().Publish(StoreItem);
            new AddStoreItemWindow().Show();
        }

        #endregion

        #region Command RemoveItem - Команда удалить предмет со склада

        private ICommand? _RemoveItemCommand;
        /// <summary>Команда - удалить предмет со склада</summary>
        public ICommand RemoveItemCommand => _RemoveItemCommand
            ??= new DelegateCommand(OnRemoveItemCommandExecuted, CanRemoveItemCommandExecute);
        private bool CanRemoveItemCommandExecute() => true;
        private async void OnRemoveItemCommandExecuted()
        {

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
