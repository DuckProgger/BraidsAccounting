﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
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
        private CollectionView collectionView = null!;
        private readonly IEventAggregator eventAggregator;
        private readonly IManufacturersService manufacturersService;
        private readonly IViewService viewService;
        private readonly IItemsService catalogue;
        private string? _colorFilter;
        private string? _manufacturerFilter;
        private string? articleFilter;
        private ICommand? _LoadDataCommand;
        private ObservableCollection<FormItem> catalogueItems = new();

        public ObservableCollection<FormItem> CatalogueItems
        {
            get => catalogueItems;
            set
            {
                catalogueItems = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(catalogueItems);
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
            , IManufacturersService manufacturersService
            , IViewService viewService
            , IItemsService catalogue
            )
        {
            this.eventAggregator = eventAggregator;
            this.manufacturersService = manufacturersService;
            this.viewService = viewService;
            this.catalogue = catalogue;
        }
        /// <summary>
        /// Предикат фильтрации списка материалов со склада.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Filter(object obj)
        {
            FormItem? item = (FormItem)obj;
            bool manufacturerCondition = string.IsNullOrEmpty(ManufacturerFilter)
                || item.Manufacturer.Contains(ManufacturerFilter, StringComparison.OrdinalIgnoreCase);
            bool articleCondition = string.IsNullOrEmpty(ArticleFilter)
                || item.Article.Contains(ArticleFilter, StringComparison.OrdinalIgnoreCase);
            bool colorCondition = string.IsNullOrEmpty(ColorFilter)
               || item.Color.Contains(ColorFilter, StringComparison.OrdinalIgnoreCase);
            return manufacturerCondition && articleCondition && colorCondition;
        }

        #region Command Select - Команда выбрать товар

        private ICommand? _SelectCommand;

        /// <summary>Команда - выбрать товар</summary>
        public ICommand SelectCommand => _SelectCommand
            ??= new DelegateCommand(OnSelectCommandExecuted, CanSelectCommandExecute);
        private bool CanSelectCommandExecute() => true;
        private void OnSelectCommandExecuted()
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
            CatalogueItems = new();
            foreach (var item in await catalogue.GetItemsAsync(OnlyInStock))
                CatalogueItems.Add(item);
            Manufacturers = await manufacturersService.GetManufacturerNamesAsync();
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
            ArticleFilter = string.Empty;
            ColorFilter = string.Empty;
            ManufacturerFilter = string.Empty;
        }

        #endregion
    }
}
