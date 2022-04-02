using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Cashbox.Visu;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class AddStoreItemViewModel : BindableBase
    {
        private ICommand? _AddStoreItemCommand;
        private readonly IStoreService store;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;
        private readonly IManufacturersService manufacturersService;

        /// <summary>
        /// Материал со склада, обрабатываемый в форме.
        /// </summary>
        public StoreItem StoreItem { get; set; } = new();
        /// <summary>
        /// Список производителей.
        /// </summary>
        public List<string>? Manufacturers { get; set; }
        /// <summary>
        /// Выбранный производитель из списка.
        /// </summary>
        public string SelectedManufacturer { get; set; } = null!;
        public int InStock { get; set; }
        /// <summary>
        /// Выводимое сообщение об ошибке.
        /// </summary>
        public MessageProvider ErrorMessage { get; } = new(true);

        public AddStoreItemViewModel(
                IStoreService store
            , IEventAggregator eventAggregator
            , IRegionManager regionManager
            , IViewService viewService
            , IManufacturersService manufacturersService
          )
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            this.viewService = viewService;
            this.manufacturersService = manufacturersService;
            StoreItem.Item = new();
            StoreItem.Item.Manufacturer = new();
            eventAggregator.GetEvent<SelectItemEvent>().Subscribe(SetStoreItem);

        }

        /// <summary>
        /// Установить <see cref = "StoreItem" />.
        /// </summary>
        /// <param name="item"></param>
        private void SetStoreItem(Item item)
        {
            if (regionManager.IsViewActive<StoreView>("ContentRegion"))
            {
                StoreItem = new()
                {
                    Item = item
                };
                InStock = store.GetItemCount(item.Manufacturer.Name, item.Article, item.Color);
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
            try
            {
                await store.AddItemAsync(StoreItem);
                viewService.GetWindow<AddStoreItemWindow>().Close();
                eventAggregator.GetEvent<ActionSuccessEvent>().Publish(true);
            }
            catch (ArgumentException)
            {
                ErrorMessage.Message = "Заполнены не все поля";
            }
        }

        #endregion

        #region Command LoadManufacturers - Команда загрузить список производителей

        private ICommand? _LoadManufacturersCommand;
        /// <summary>Команда - загрузить список производителей</summary>
        public ICommand LoadManufacturersCommand => _LoadManufacturersCommand
            ??= new DelegateCommand(OnLoadManufacturersCommandExecuted, CanLoadManufacturersCommandExecute);
        private bool CanLoadManufacturersCommandExecute() => true;
        private async void OnLoadManufacturersCommandExecuted() => Manufacturers = new(await manufacturersService.GetManufacturerNamesAsync());

        #endregion

        #region Command SelectStoreItem - Команда выбрать товар со склада

        private ICommand? _SelectStoreItemCommand;
        /// <summary>Команда - выбрать товар со склада</summary>
        public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
            ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
        private bool CanSelectStoreItemCommandExecute() => true;
        private void OnSelectStoreItemCommandExecuted() => viewService.ActivateWindowWithClosing<ItemsCatalogueWindow, AddStoreItemWindow>();

        #endregion
    }
}
