using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
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
        //private Service service = new();
        private WastedItem wastedItem = new();

        public Service Service { get; set; } = new();
        //{
        //    get => service;
        //    set
        //    {
        //        service = value;
        //    }
        //}
        public ObservableCollection<WastedItem> WastedItems { get; set; } = new();
        public WastedItem WastedItem
        {
            get => wastedItem;
            set
            {
                //int maxCount = sto
                wastedItem = value;
            }
        }
        private readonly Services.Interfaces.IServiceProvider serviceProvider;
        private readonly IRepository<WastedItem> wastedItemsRepository;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;

        public ServiceViewModel(
            Services.Interfaces.IServiceProvider serviceProvider
            , IEventAggregator eventAggregator
            , IRepository<WastedItem> wastedItemsRepository
            , IRegionManager regionManager
            , IViewService viewService

            )
        {
            this.serviceProvider = serviceProvider;
            this.wastedItemsRepository = wastedItemsRepository;
            this.regionManager = regionManager;
            this.viewService = viewService;
            eventAggregator.GetEvent<SelectStoreItemEvent>().Subscribe(MessageReceived);
        }

        private void MessageReceived(StoreItem? storeItem)
        {
            if (storeItem is not null && regionManager.IsViewActive<ServiceView>("ContentRegion"))
            {
                WastedItem wastedItem = new()
                {
                    Item = storeItem.Item
                };
                WastedItems.Add(wastedItem);
            }
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
            viewService.ActivateWindowWithClosing<SelectStoreItemWindow, MainWindow>();
        }

        #endregion
    }
}
