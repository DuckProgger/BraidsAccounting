using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Cashbox.Visu;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class ItemsCatalogueViewModel : BindableBase
    {
        private CollectionView? collectionView;
        private readonly IEventAggregator eventAggregator;
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;
        private readonly IViewService viewService;
        private readonly IItemsService catalogue;
        private string? _colorFilter;
        private string? _manufacturerFilter;
        private string? articleFilter;
        private ICommand? _LoadDataCommand;
        private ObservableCollection<FormItem> storeItems = new();

        public ObservableCollection<FormItem> CatalogueItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(storeItems);
                collectionView.Filter = Filter;
            }
        }
        /// <summary>
        /// Выбранный материал из каталога.
        /// </summary>
        public FormItem SelectedItem { get; set; } = null!;
        /// <summary>
        /// Значение, введённое в поле фильтра артикула.
        /// </summary>
        public string? ArticleFilter
        {
            get => articleFilter;
            set
            {
                articleFilter = value;
                collectionView.Refresh();
            }
        }
        /// <summary>
        /// Флаг фильтрации отображаемых элементов каталога
        /// материалов - только в наличии.
        /// </summary>
        public bool OnlyInStock { get; set; } = false;
        /// <summary>
        /// Значение, введённое в поле фильтра цвета.
        /// </summary>
        public string? ColorFilter
        {
            get => _colorFilter;
            set
            {
                _colorFilter = value;
                collectionView.Refresh();
            }
        }
        /// <summary>
        /// Значение, введённое в поле фильтра производителя.
        /// </summary>
        public string? ManufacturerFilter
        {
            get => _manufacturerFilter;
            set
            {
                _manufacturerFilter = value;
                collectionView?.Refresh();
            }
        }
        /// <summary>
        /// Список производителей
        /// </summary>
        public List<string> Manufacturers { get; set; } = null!;
        /// <summary>
        /// Выводимое сообщение об ошибке.
        /// </summary>
        public MessageProvider ErrorMessage { get; } = new(true);

        public ItemsCatalogueViewModel(
            IEventAggregator eventAggregator
            , IStoreService store
            , IManufacturersService manufacturersService
            , IViewService viewService
            , IItemsService itemsService
            )
        {
            this.eventAggregator = eventAggregator;
            this.store = store;
            this.manufacturersService = manufacturersService;
            this.viewService = viewService;
            this.catalogue = itemsService;
        }
        /// <summary>
        /// Предикат фильтрации списка материалов со склада.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Filter(object obj)
        {
            StoreItem? item = (StoreItem)obj;
            bool manufacturerCondition = string.IsNullOrEmpty(ManufacturerFilter)
                || item.Item.Manufacturer.Name.Contains(ManufacturerFilter, StringComparison.OrdinalIgnoreCase);
            bool articleCondition = string.IsNullOrEmpty(ArticleFilter)
                || item.Item.Article.Contains(ArticleFilter, StringComparison.OrdinalIgnoreCase);
            bool colorCondition = string.IsNullOrEmpty(ColorFilter)
               || item.Item.Color.Contains(ColorFilter, StringComparison.OrdinalIgnoreCase);
            return manufacturerCondition && articleCondition && colorCondition;
        }

        #region Command Select - Команда выбрать товар

        private ICommand? _SelectCommand;

        /// <summary>Команда - выбрать товар</summary>
        public ICommand SelectCommand => _SelectCommand
            ??= new DelegateCommand(OnSelectCommandExecuted, CanSelectCommandExecute);
        private bool CanSelectCommandExecute() => true;
        private async void OnSelectCommandExecuted()
        {
            if (SelectedItem is null)
            {
                ErrorMessage.Message = "Не выбран ни один товар";
                return;
            }
            eventAggregator.GetEvent<SelectItemEvent>().Publish(SelectedItem);
            viewService.GetWindow<ItemsCatalogueWindow>().Close();
        }

        #endregion

        #region Command LoadData - Команда загрузки данных со склада

        /// <summary>Команда - загрузки данных со склада</summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
        private bool CanLoadDataCommandExecute() => true;
        private async void OnLoadDataCommandExecuted() => await LoadData();

        private async Task LoadData()
        {
            CatalogueItems.Clear();
            foreach (var item in catalogue.GetItems(OnlyInStock))
                CatalogueItems.Add(item);
            Manufacturers = new(await manufacturersService.GetManufacturerNamesAsync());
        }

        #endregion

        #region Command ResetFiltersCommand - Команда сбросить фильтры

        private ICommand? _ResetFiltersCommand;
        /// <summary>Команда - сбросить фильтры</summary>
        public ICommand ResetFiltersCommand => _ResetFiltersCommand
            ??= new DelegateCommand(OnResetFiltersCommandExecuted, CanResetFiltersCommandExecute);
        private bool CanResetFiltersCommandExecute() => true;
        private async void OnResetFiltersCommandExecuted()
        {
            ArticleFilter = string.Empty;
            ColorFilter = string.Empty;
            ManufacturerFilter = string.Empty;
        }

        #endregion
    }
}
