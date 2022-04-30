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
    internal class SelectItemViewModel : ViewModelBase
    {
        private readonly IViewService viewService;

        private ICommand? _LoadDataCommand;
        public SelectItemViewModel(IViewService viewService)
        {
            this.viewService = viewService;
        }

        /// <summary>
        /// Выбранный материал из каталога.
        /// </summary>
        public Item SelectedItem { get; set; } = null!;

        /// <summary>
        /// Флаг фильтрации отображаемых элементов каталога
        /// материалов - только в наличии.
        /// </summary>
        public bool OnlyInStock { get; set; } = false;

        public List<string> Manufacturers { get; set; } = null!;

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
                //Error.Message = "Не выбран ни один товар";
                return;
            }
            //var parameters = new NavigationParameters();
            //if (SelectedItem is not null)
            //    parameters.Add("item", SelectedItem);
            viewService.AddParameter("item", SelectedItem);
            viewService.GoBack();
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
