﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class SelectItemViewModel : FilterableBindableBase<FormItem>, ISignaling
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IManufacturersService manufacturersService;
        private readonly IViewService viewService;
        private readonly ICatalogueService catalogue;
        private string? _colorFilter;
        private string? _manufacturerFilter;
        private string? articleFilter;
        private ICommand? _LoadDataCommand;
        public SelectItemViewModel(
           IEventAggregator eventAggregator
           , IManufacturersService manufacturersService
           , IViewService viewService
           , ICatalogueService catalogue
           )
        {
            this.eventAggregator = eventAggregator;
            this.manufacturersService = manufacturersService;
            this.viewService = viewService;
            this.catalogue = catalogue;
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

        #region Messages

        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider StatusMessage => throw new NotImplementedException();
        public MessageProvider WarningMessage => throw new NotImplementedException();

        #endregion       

        protected override bool Filter(object obj)
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
            viewService.GetWindow<SelectItemWindow>().Close();
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
            Collection = new();
            foreach (Item? item in await catalogue.GetAllAsync(OnlyInStock))
                Collection.Add(item);
            Manufacturers = await manufacturersService.GetNamesAsync();
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