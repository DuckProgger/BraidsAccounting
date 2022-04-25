﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class AddItemViewModel : BindableBase, ISignaling
    {
        private readonly ICatalogueService catalogueService;
        private readonly IViewService viewService;

        public AddItemViewModel(ICatalogueService catalogueService, IViewService viewService)
        {
            this.catalogueService = catalogueService;
            this.viewService = viewService;
            ItemInForm.Manufacturer = new();
        }
        /// <summary>
        /// Материал со склада, обрабатываемый в форме.
        /// </summary>
        public Item ItemInForm { get; set; } = new();
        /// <summary>
        /// Список производителей.
        /// </summary>
        public List<string>? Manufacturers { get; set; }
        /// <summary>
        /// Выбранный производитель из списка.
        /// </summary>
        public string SelectedManufacturer { get; set; } = null!;

        #region Messages
        public MessageProvider StatusMessage => throw new NotImplementedException();
        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider WarningMessage => throw new NotImplementedException();
        #endregion

        private static bool IsValidItem(Item item) =>
             !string.IsNullOrEmpty(item.Manufacturer.Name)
            && !string.IsNullOrEmpty(item.Color)
            && !string.IsNullOrEmpty(item.Article);

        #region Command AddItem - Команда добавить новый материал в каталог

        private ICommand? _AddItemCommand;
        /// <summary>Команда - добавить новый материал в каталог</summary>
        public ICommand AddItemCommand => _AddItemCommand
            ??= new DelegateCommand(OnAddItemCommandExecuted, CanAddItemCommandExecute);
        private bool CanAddItemCommandExecute() => true;
        private async void OnAddItemCommandExecuted()
        {
            ItemInForm.Manufacturer.Name = SelectedManufacturer;
            if (!IsValidItem(ItemInForm))
            {
                ErrorMessage.Message = "Заполнены не все поля";
                return;
            }
            await catalogueService.AddAsync(ItemInForm);
            viewService.GetWindow<AddItemWindow>().Close();

        }

        #endregion

        #region Command InitializeData - Команда заполнить форму начальными данными

        private ICommand? _InitializeDataCommand;

        /// <summary>Команда - заполнить форму начальными данными</summary>
        public ICommand InitializeDataCommand => _InitializeDataCommand
            ??= new DelegateCommand(OnInitialDataCommandExecuted, CanInitialDataCommandExecute);

        private bool CanInitialDataCommandExecute() => true;
        private async void OnInitialDataCommandExecuted()
        {
            var manufacturersService = ServiceLocator.GetService<IManufacturersService>();
            var employees = await manufacturersService.GetAllAsync();
            Manufacturers = new(employees.Select(e => e.Name));
        }

        #endregion
    }
}