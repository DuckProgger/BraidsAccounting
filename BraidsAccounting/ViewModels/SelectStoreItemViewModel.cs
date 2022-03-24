using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure.Events;
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
using System.Windows.Data;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class SelectStoreItemViewModel : BindableBase
    {
        public ObservableCollection<StoreItem> StoreItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(storeItems);
                collectionView.Filter = Filter;
            }
        }
        public StoreItem SelectedItem { get; set; }
        public string ArticleFilter 
        { 
            get => articleFilter;
            set
            {
                articleFilter = value;
                collectionView.Refresh();
            }
        }

        private string _colorFilter;
        public string ColorFilter
        {
            get => _colorFilter;
            set
            {
                _colorFilter = value;
                collectionView.Refresh();
            }
        }

        private string _manufacturerFilter;
        public string ManufacturerFilter
        {
            get => _manufacturerFilter;
            set
            {
                _manufacturerFilter = value;
                collectionView.Refresh();
            }
        }

        public ObservableCollection<string> Manufacturers { get; set; }


        private CollectionView collectionView;
        private readonly IEventAggregator eventAggregator;
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;

        public SelectStoreItemViewModel(
            IEventAggregator eventAggregator
            , IStoreService store
            , IManufacturersService manufacturersService
            )
        {
            this.eventAggregator = eventAggregator;
            this.store = store;
            this.manufacturersService = manufacturersService;
        }

        public bool Filter(object obj)
        {
            var item = obj as StoreItem;
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
            eventAggregator.GetEvent<SelectStoreItemEvent>().Publish(SelectedItem);
        }

        #endregion

        #region Command LoadData - Команда загрузки данных со склада

        private ICommand? _LoadDataCommand;
        private ObservableCollection<StoreItem> storeItems = new();
        private string articleFilter;

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
            Manufacturers = new(manufacturersService.GetManufacturerNames());
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
