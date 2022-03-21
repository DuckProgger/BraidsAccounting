using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Views;
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
    internal class ServiceViewModel : BindableBase
    {
        public Service Service { get; set; } = new();
        public ObservableCollection<WastedItem> WastedItems { get; set; } = new();
        public WastedItem WastedItem { get; set; } = new();

        //public string Name { get; set; } = null!;
        //public decimal Profit { get; set; }

        private readonly Services.Interfaces.IServiceProvider serviceProvider;
        private readonly IRepository<WastedItem> wastedItemsRepository;

        public ServiceViewModel(
            Services.Interfaces.IServiceProvider serviceProvider
            , IEventAggregator eventAggregator
            , IRepository<WastedItem> wastedItemsRepository
            )
        {
            this.serviceProvider = serviceProvider;
            this.wastedItemsRepository = wastedItemsRepository;
            eventAggregator.GetEvent<PubSubEvent<StoreItem>>().Subscribe(MessageReceived);
        }

        private void MessageReceived(StoreItem storeItem)
        {
            WastedItem wastedItem = new()
            {
                Item = storeItem.Item
            };
            WastedItems.Add(wastedItem);
        }

        #region Command CreateService - Добавление сервиса

        private ICommand _CreateServiceCommand;
        /// <summary>Команда - Добавление сервиса</summary>
        public ICommand CreateServiceCommand => _CreateServiceCommand
            ??= new DelegateCommand(OnCreateServiceCommandExecuted, CanCreateServiceCommandExecute);
        private bool CanCreateServiceCommandExecute() => true;
        private async void OnCreateServiceCommandExecuted()
        {
            Service.WastedItems = new(WastedItems);
            serviceProvider.ProvideService(Service);
        }

        #endregion

        #region Command SelectStoreItem - Команда выбрать товар со склада

        private ICommand? _SelectStoreItemCommand;
        /// <summary>Команда - выбрать товар со склада</summary>
        public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
            ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
        private bool CanSelectStoreItemCommandExecute() => true;
        private async void OnSelectStoreItemCommandExecuted()
        {
            new SelectStoreItemView().Show();
        }

        #endregion
    }
}
