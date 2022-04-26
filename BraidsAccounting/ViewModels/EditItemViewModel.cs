using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditItemViewModel : BindableBase, ISignaling, INavigationAware
    {
        private ICommand? _SaveChangesCommand;
        private readonly ICatalogueService catalogueService;
        private readonly IViewService viewService;
        private readonly IEventAggregator eventAggregator;
        private readonly IManufacturersService manufacturersService;

        public EditItemViewModel(
            ICatalogueService catalogueService
            , IViewService viewService
            , IEventAggregator eventAggregator
            , IManufacturersService manufacturersService
            )
        {
            this.catalogueService = catalogueService;
            this.viewService = viewService;
            this.eventAggregator = eventAggregator;
            this.manufacturersService = manufacturersService;
            //eventAggregator.GetEvent<EditItemEvent>().Subscribe(SetProperties);
        }

        ///// <summary>
        ///// Устанавливает свойства при приёме сообщения.
        ///// </summary>
        ///// <param name="item"></param>
        //private async void SetProperties(Item item)
        //{
        //    ItemInForm = item;
        //    Manufacturers = await manufacturersService.GetAllAsync();
        //    SelectedManufacturer = item.Manufacturer;
        //}

        /// <summary>
        /// Материал из каталога, обрабатываемый в форме.
        /// </summary>
        public Item ItemInForm { get; set; } = new();
        /// <summary>
        /// Список производителей.
        /// </summary>
        public List<Manufacturer>? Manufacturers { get; set; }
        /// <summary>
        /// Выбранный производитель из списка.
        /// </summary>
        public Manufacturer SelectedManufacturer { get; set; } = null!;

        #region Messages

        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider StatusMessage => throw new NotImplementedException();
        public MessageProvider WarningMessage => throw new NotImplementedException();

        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext) 
        {
            var item = navigationContext.Parameters["item"] as Item;
            if (item is not null) ItemInForm = item;
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #region Command SaveChanges - Команда сохранить изменения товара со склада

        /// <summary>Команда - сохранить изменения товара со склада</summary>
        public ICommand SaveChangesCommand => _SaveChangesCommand
            ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);

        private bool CanSaveChangesCommandExecute() => true;
        private async void OnSaveChangesCommandExecuted()
        {
            try
            {
                ItemInForm.Manufacturer = SelectedManufacturer;
                await catalogueService.EditAsync(ItemInForm);
                viewService.GetWindow<EditItemWindow>().Close();
                eventAggregator.GetEvent<ActionSuccessEvent>().Publish(true);
            }
            catch (ArgumentException)
            {
                ErrorMessage.Message = "Заполнены не все поля";
            }
        }    

        #endregion
    }
}
