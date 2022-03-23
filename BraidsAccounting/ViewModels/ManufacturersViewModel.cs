using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class ManufacturersViewModel
    {
        private readonly IManufacturersService manufacturersService;
        private readonly IItemsService itemsService;

        public bool DialogResult { get; set; }
        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public Manufacturer SelectedManufacturer { get; set; }
        public Manufacturer ManufacturerInForm { get; set; }


        public ManufacturersViewModel(
            IManufacturersService manufacturersService,
            IItemsService itemsService
            )
        {
            this.manufacturersService = manufacturersService;
            this.itemsService = itemsService;
        }

        #region Command GetManufacturersList - Команда получить всех производителей

        private ICommand? _GetManufacturersListCommand;
        /// <summary>Команда - получить всех производителей</summary>
        public ICommand GetManufacturersListCommand => _GetManufacturersListCommand
            ??= new DelegateCommand(OnGetManufacturersListCommandExecuted, CanGetManufacturersListCommandExecute);
        private bool CanGetManufacturersListCommandExecute() => true;
        private async void OnGetManufacturersListCommandExecuted()
        {
            Manufacturers = new(manufacturersService.GetManufacturers());
        }

        #endregion

        #region Command Save - Команда сохранить изменения

        private ICommand? _SaveCommand;
        /// <summary>Команда - сохранить изменения</summary>
        public ICommand SaveCommand => _SaveCommand
            ??= new DelegateCommand<Manufacturer>(OnSaveCommandExecuted, CanSaveCommandExecute);
        private bool CanSaveCommandExecute(Manufacturer manufacturer) => true;
        private async void OnSaveCommandExecuted(Manufacturer manufacturer)
        {
            switch (manufacturer.Id)
            {
                case 0:
                    manufacturersService.AddManufacturer(manufacturer);
                    break;
                default:
                    manufacturersService.RemoveManufacturer(manufacturer.Id);
                    break;
            }
        }



        #endregion

        #region Command RemoveManufacturer - Команда удалить производителя с ценой

        private ICommand? _RemoveManufacturerCommand;
        /// <summary>Команда - удалить производителя с ценой</summary>
        public ICommand RemoveManufacturerCommand => _RemoveManufacturerCommand
            ??= new DelegateCommand<Manufacturer>(OnRemoveManufacturerCommandExecuted, CanRemoveManufacturerCommandExecute);
        private bool CanRemoveManufacturerCommandExecute(Manufacturer manufacturer) => true;
        private async void OnRemoveManufacturerCommandExecuted(Manufacturer manufacturer)
        {
            if (itemsService.ContainsManufacturer(manufacturer.Name))
            {
                // Предупредить, что в каталоге содержится товар производителя
                // и при удалении удалятся все связанные товары из каталога и со склада
                //if(!DialogResult) // не удалять
            }
            manufacturersService.RemoveManufacturer(manufacturer.Id);
        }

        #endregion

        #region Command ResetForm - Команда сбросить форму

        private ICommand? _ResetFormCommand;
        /// <summary>Команда - сбросить форму</summary>
        public ICommand ResetFormCommand => _ResetFormCommand
            ??= new DelegateCommand(OnResetFormCommandExecuted, CanResetFormCommandExecute);
        private bool CanResetFormCommandExecute() => true;
        private async void OnResetFormCommandExecuted()
        {
            ManufacturerInForm = new();
        }

        #endregion

        #region Command FillForm - Команда заполнить форму выбранным производителем для редактирования

        private ICommand? _FillFormCommand;
        /// <summary>Команда - заполнить форму выбранным производителем для редактирования</summary>
        public ICommand FillFormCommand => _FillFormCommand
            ??= new DelegateCommand(OnFillFormCommandExecuted, CanFillFormCommandExecute);
        private bool CanFillFormCommandExecute() => true;
        private async void OnFillFormCommandExecuted()
        {
            //ManufacturerInForm = new() { };
        }

        #endregion
    }
}

