﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels
{
    internal class ManufacturersViewModel : FilterableBindableBase<Manufacturer>, INotifying
    {
        private readonly IManufacturersService manufacturersService;
        private readonly ICatalogueService itemsService;
        private string? _manufacturerFilter;

        public ManufacturersViewModel(
           IManufacturersService manufacturersService,
           ICatalogueService itemsService
           )
        {
            this.manufacturersService = manufacturersService;
            this.itemsService = itemsService;
        }
        public string Title => "Производители";

        /// <summary>
        /// Выбранный производитель в представлении.
        /// </summary>
        public Manufacturer SelectedManufacturer { get; set; } = null!;
        /// <summary>
        /// Отображаемый в представлении прозводитель.
        /// </summary>
        public Manufacturer ManufacturerInForm { get; set; } = new();

        /// <summary>
        /// Список производителей в представлении.
        /// </summary>
        public ObservableCollection<string> ManufacturerList { get; set; } = null!;
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

        #region Messages

        public Notifier Status { get; } = new(true);
        public Notifier Error { get; } = new(true);
        public Notifier Warning { get; } = new();

        #endregion

        protected override bool Filter(object obj)
        {
            Manufacturer item = (Manufacturer)obj;
            bool manufacturerCondition = string.IsNullOrEmpty(ManufacturerFilter)
                || item.Name.Contains(ManufacturerFilter, StringComparison.OrdinalIgnoreCase);
            return manufacturerCondition;
        }

        #region Command GetManufacturersList - Команда получить всех производителей

        private ICommand? _GetManufacturersListCommand;
        /// <summary>Команда - получить всех производителей</summary>
        public ICommand GetManufacturersListCommand => _GetManufacturersListCommand
            ??= new DelegateCommand(OnGetManufacturersListCommandExecuted, CanGetManufacturersListCommandExecute);
        private bool CanGetManufacturersListCommandExecute() => true;
        private async void OnGetManufacturersListCommandExecuted()
        {
            Collection = new(await manufacturersService.GetAllAsync());
            ManufacturerList = new(await manufacturersService.GetNamesAsync());
        }

        #endregion

        #region Command Save - Команда сохранить изменения

        private ICommand? _SaveCommand;
        /// <summary>Команда - сохранить изменения</summary>
        public ICommand SaveCommand => _SaveCommand
            ??= new DelegateCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
        private bool CanSaveCommandExecute() => true;
        private async void OnSaveCommandExecuted()
        {
            try
            {
                switch (ManufacturerInForm.Id)
                {
                    case 0:
                        await manufacturersService.AddAsync(ManufacturerInForm);
                        Collection.Add(ManufacturerInForm);
                        Status.Message = "Новый производитель добавлен";
                        break;
                    default:
                        await manufacturersService.EditAsync(ManufacturerInForm);
                        Status.Message = "Производитель изменён";
                        break;
                }
                OnGetManufacturersListCommandExecuted();
                ResetFormCommand.Execute(null);
            }
            catch (ArgumentException)
            {
                Error.Message = "Не все поля заполнены";
            }
        }

        #endregion

        #region Command RemoveManufacturer - Команда удалить производителя с ценой

        private ICommand? _RemoveManufacturerCommand;
        /// <summary>Команда - удалить производителя с ценой</summary>
        public ICommand RemoveManufacturerCommand => _RemoveManufacturerCommand
            ??= new DelegateCommand(OnRemoveManufacturerCommandExecuted, CanRemoveManufacturerCommandExecute);
        private bool CanRemoveManufacturerCommandExecute() => true;
        private async void OnRemoveManufacturerCommandExecuted()
        {
            await manufacturersService.RemoveAsync(SelectedManufacturer.Id);
            Collection.Remove(SelectedManufacturer);
            Status.Message = "Производитель удалён";
            MDDialogHost.CloseDialogCommand.Execute(null, null);
        }

        #endregion

        #region Command ResetForm - Команда сбросить форму

        private ICommand? _ResetFormCommand;
        /// <summary>Команда - сбросить форму</summary>
        public ICommand ResetFormCommand => _ResetFormCommand
            ??= new DelegateCommand(OnResetFormCommandExecuted, CanResetFormCommandExecute);
        private bool CanResetFormCommandExecute() => true;
        private void OnResetFormCommandExecuted() => ManufacturerInForm = new();

        #endregion

        #region Command FillForm - Команда заполнить форму выбранным производителем для редактирования

        private ICommand? _FillFormCommand;
        /// <summary>Команда - заполнить форму выбранным производителем для редактирования</summary>
        public ICommand FillFormCommand => _FillFormCommand
            ??= new DelegateCommand(OnFillFormCommandExecuted, CanFillFormCommandExecute);
        private bool CanFillFormCommandExecute() => true;
        private void OnFillFormCommandExecuted()
        {
            ManufacturerInForm = new()
            {
                Id = SelectedManufacturer.Id,
                Name = SelectedManufacturer.Name,
                Price = SelectedManufacturer.Price
            };
        }

        #endregion

        #region Command OpenDialog - Команда открыть диалог

        private ICommand? _OpenDialogCommand;
        /// <summary>Команда - открыть диалог</summary>
        public ICommand OpenDialogCommand => _OpenDialogCommand
            ??= new DelegateCommand(OnOpenDialogCommandExecuted, CanOpenDialogCommandExecute);
        private bool CanOpenDialogCommandExecute() => true;
        private void OnOpenDialogCommandExecuted()
        {
            MDDialogHost.OpenDialogCommand.Execute(null, null);
            Warning.Message = itemsService.ContainsManufacturer(SelectedManufacturer.Name)
                ? "В списке есть товары выбранной фирмы!"
                : string.Empty;
        }

        #endregion

        #region Command ResetFiltersCommand - Команда сбросить фильтры

        private ICommand? _ResetFiltersCommand;
        /// <summary>Команда - сбросить фильтры</summary>
        public ICommand ResetFiltersCommand => _ResetFiltersCommand
            ??= new DelegateCommand(OnResetFiltersCommandExecuted, CanResetFiltersCommandExecute);
        private bool CanResetFiltersCommandExecute() => true;
        private void OnResetFiltersCommandExecuted() => ManufacturerFilter = string.Empty;

        #endregion
    }
}

