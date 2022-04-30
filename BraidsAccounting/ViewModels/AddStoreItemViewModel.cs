using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Modules;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class AddStoreItemViewModel : ViewModelBase
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
        }

        /// <summary>
        /// Установить <see cref = "StoreItem" />.
        /// </summary>
        /// <param name="item"></param>
        private void SetStoreItem(Item item)
        {
            if (regionManager.IsViewActive<StoreView>(RegionNames.Catalogs))
            {
                StoreItem = new()
                {
                    Item = item
                };
                InStock = store.GetItemCount(item.Manufacturer.Name, item.Article, item.Color);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Item? item = navigationContext.Parameters["item"] as Item;
            if (item is not null)
            {
                SetStoreItem(item);
                SelectedManufacturer = item.Manufacturer.Name;
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
                viewService.AddParameter(ParameterNames.AddItemResult, true);
                viewService.GoBack();
            }
            catch (ArgumentException)
            {
                Notifier.AddError(MessageContainer.Get("fieldsNotFilled"));
            }
        }

        #endregion

        #region Command LoadManufacturers - Команда загрузить список производителей

        private ICommand? _LoadManufacturersCommand;
        /// <summary>Команда - загрузить список производителей</summary>
        public ICommand LoadManufacturersCommand => _LoadManufacturersCommand
            ??= new DelegateCommand(OnLoadManufacturersCommandExecuted, CanLoadManufacturersCommandExecute);
        private bool CanLoadManufacturersCommandExecute() => true;
        private async void OnLoadManufacturersCommandExecuted() => Manufacturers = await manufacturersService.GetNamesAsync();

        #endregion

        #region Command SelectStoreItem - Команда выбрать товар со склада

        private ICommand? _SelectStoreItemCommand;
        /// <summary>Команда - выбрать товар со склада</summary>
        public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
            ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
        private bool CanSelectStoreItemCommandExecute() => true;
        private void OnSelectStoreItemCommandExecuted() =>
            viewService.ShowPopupWindow(nameof(SelectItemView));

        //viewService.ShowPopupWindow(nameof(SelectItemView));


        //viewService.ShowPopupWindow(nameof(CatalogsView), o => SetStoreItem(o as Item));
        //viewService.ShowWindowWithClosing<SelectItemWindow, AddStoreItemWindow>();

        #endregion

    }
}
