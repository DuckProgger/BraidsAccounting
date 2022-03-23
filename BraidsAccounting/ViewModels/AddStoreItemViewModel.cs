using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
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
    internal class AddStoreItemViewModel : BindableBase
    {
        private ICommand? _AddStoreItemCommand;
        private readonly IStoreService store;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;
        private readonly IManufacturersService manufacturersService;

        public StoreItem StoreItem { get; set; } = new();
        public ObservableCollection<string> Manufacturers { get; set; }
        public string SelectedManufacturer { get; set; }

        public AddStoreItemViewModel(
                IStoreService store
            , IEventAggregator eventAggregator
            , IRegionManager regionManager
            , IViewService viewService
            , IManufacturersService manufacturersService
          )
        {
            this.store = store;
            this.regionManager = regionManager;
            this.viewService = viewService;
            this.manufacturersService = manufacturersService;
            StoreItem.Item = new();
            StoreItem.Item.Manufacturer = new();
            eventAggregator.GetEvent<SelectStoreItemEvent>().Subscribe(MessageReceived);

        }
        private void MessageReceived(StoreItem storeItem)
        {
            if (regionManager.IsViewActive<StoreView>("ContentRegion"))
            {
                StoreItem = new()
                {
                    Item = storeItem.Item
                };
            }    
        }

        #region Command AddStoreItem - Команда добавить товар на склад

        /// <summary>Команда - добавить товар на склад</summary>
        public ICommand AddStoreItemCommand => _AddStoreItemCommand
            ??= new DelegateCommand(OnAddStoreItemCommandExecuted, CanAddStoreItemCommandExecute);
        private bool CanAddStoreItemCommandExecute() => true;
        private async void OnAddStoreItemCommandExecuted()
        {
            StoreItem.Item.Manufacturer.Name = SelectedManufacturer;
            store.AddItem(StoreItem);
        }

        #endregion

        #region Command LoadManufacturers - Команда загрузить список производителей

        private ICommand? _LoadManufacturersCommand;
        /// <summary>Команда - загрузить список производителей</summary>
        public ICommand LoadManufacturersCommand => _LoadManufacturersCommand
            ??= new DelegateCommand(OnLoadManufacturersCommandExecuted, CanLoadManufacturersCommandExecute);
        private bool CanLoadManufacturersCommandExecute() => true;
        private async void OnLoadManufacturersCommandExecuted()
        {
            Manufacturers = new(manufacturersService.GetManufacturerNames());
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
            viewService.ActivateWindowWithClosing<SelectStoreItemWindow, AddStoreItemWindow>();
        }

        #endregion
    }
}
