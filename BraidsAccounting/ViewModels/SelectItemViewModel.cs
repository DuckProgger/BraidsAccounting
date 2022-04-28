using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class SelectItemViewModel : BindableBase, INotifying
    {
        private readonly IEventAggregator eventAggregator;
        //private readonly IManufacturersService manufacturersService;
        private readonly IViewService viewService;
        //private readonly ICatalogueService catalogue;
        //private string? _colorFilter;
        //private string? _manufacturerFilter;
        //private string? articleFilter;
        private ICommand? _LoadDataCommand;
        public SelectItemViewModel(
           IEventAggregator eventAggregator
           //, IManufacturersService manufacturersService
           , IViewService viewService
           //, ICatalogueService catalogue
           )
        {
            this.eventAggregator = eventAggregator;
            //this.manufacturersService = manufacturersService;
            this.viewService = viewService;
            //this.catalogue = catalogue;
        }

        /// <summary>
        /// Выбранный материал из каталога.
        /// </summary>
        public Item SelectedItem { get; set; } = null!;
        /// <summary>
        /// Значение, введённое в поле фильтра артикула.
        /// </summary>
        //public string? ArticleFilter { get; set; }
        /// <summary>
        /// Флаг фильтрации отображаемых элементов каталога
        /// материалов - только в наличии.
        /// </summary>
        public bool OnlyInStock { get; set; } = false;
        /// <summary>
        /// Значение, введённое в поле фильтра цвета.
        /// </summary>
        //public string? ColorFilter
        //{
        //    get => _colorFilter;
        //    set
        //    {
        //        _colorFilter = value;
        //        collectionView.Refresh();
        //    }
        //}
        /// <summary>
        /// Значение, введённое в поле фильтра производителя.
        /// </summary>
        //public string? ManufacturerFilter
        //{
        //    get => _manufacturerFilter;
        //    set
        //    {
        //        _manufacturerFilter = value;
        //        collectionView?.Refresh();
        //    }
        //}
        /// <summary>
        /// Список производителей
        /// </summary>
        public List<string> Manufacturers { get; set; } = null!;

        #region Messages

        public Notifier Error { get; } = new(true);
        public Notifier Status => throw new NotImplementedException();
        public Notifier Warning => throw new NotImplementedException();

        #endregion       

        //protected override bool Filter(object obj)
        //{
        //    FormItem? item = (FormItem)obj;
        //    bool manufacturerCondition = string.IsNullOrEmpty(ManufacturerFilter)
        //        || item.Manufacturer.Contains(ManufacturerFilter, StringComparison.OrdinalIgnoreCase);
        //    bool articleCondition = string.IsNullOrEmpty(ArticleFilter)
        //        || item.Article.Contains(ArticleFilter, StringComparison.OrdinalIgnoreCase);
        //    bool colorCondition = string.IsNullOrEmpty(ColorFilter)
        //       || item.Color.Contains(ColorFilter, StringComparison.OrdinalIgnoreCase);
        //    return manufacturerCondition && articleCondition && colorCondition;
        //}

        #region Command Select - Команда выбрать товар

        private ICommand? _SelectCommand;

        /// <summary>Команда - выбрать товар</summary>
        public ICommand SelectCommand => _SelectCommand
            ??= new DelegateCommand<Item>(OnSelectCommandExecuted, CanSelectCommandExecute);
        private bool CanSelectCommandExecute(Item item) => true;
        private void OnSelectCommandExecuted(Item item)
        {
            SelectedItem = item;
            if (SelectedItem is null)
            {
                Error.Message = "Не выбран ни один товар";
                return;
            }
            var parameters = new NavigationParameters();
            if (SelectedItem is not null)
                parameters.Add("item", SelectedItem);
            //viewService.GoBack(parameters);
            //eventAggregator.GetEvent<SelectItemEvent>().Publish(SelectedItem);
            //viewService.GetWindow<SelectItemWindow>().Close();
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
            //Collection = new();
            //foreach (Item? item in await catalogue.GetAllAsync(OnlyInStock))
            //    Collection.Add(item);
            //Manufacturers = await manufacturersService.GetNamesAsync();
        }

        #endregion

        #region Command ResetFiltersCommand - Команда сбросить фильтры

        private ICommand? _ResetFiltersCommand;
        /// <summary>Команда - сбросить фильтры</summary>
        public ICommand ResetFiltersCommand => _ResetFiltersCommand
            ??= new DelegateCommand(OnResetFiltersCommandExecuted, CanResetFiltersCommandExecute);

        private bool CanResetFiltersCommandExecute() => true;
        private void OnResetFiltersCommandExecuted()
        {
            //ArticleFilter = string.Empty;
            //ColorFilter = string.Empty;
            //ManufacturerFilter = string.Empty;
        }

        #endregion

        #region Command GoBack - Команда перейти на предыдущий экран

        private ICommand? _GoBackCommand;
        /// <summary>Команда - перейти на предыдущий экран</summary>
        public ICommand GoBackCommand => _GoBackCommand
            ??= new DelegateCommand(OnGoBackCommandExecuted, CanGoBackCommandExecute);
        private bool CanGoBackCommandExecute() => true;
        private void OnGoBackCommandExecuted()
        {
            viewService.GoBack();
        }

        #endregion

    }
}
