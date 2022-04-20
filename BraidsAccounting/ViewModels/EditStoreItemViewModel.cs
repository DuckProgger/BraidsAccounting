using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditStoreItemViewModel : BindableBase
    {
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewService viewService;

        /// <summary>
        /// Материал со склада, обрабатываемый в форме.
        /// </summary>
        public StoreItem StoreItem { get; set; } = new();
        /// <summary>
        /// Список производителей.
        /// </summary>
        public List<string>? Manufacturers { get; set; }
        /// <summary>
        /// Выбранный производитель из списка.
        /// </summary>
        public string SelectedManufacturer { get; set; } = null!;
        /// <summary>
        /// Выводимое сообщение об ошибке.
        /// </summary>
        public MessageProvider ErrorMessage { get; } = new(true);


        public EditStoreItemViewModel(
         IStoreService store
            , IManufacturersService manufacturersService
            , IEventAggregator eventAggregator
            , IViewService viewService
         )
        {
            this.store = store;
            this.manufacturersService = manufacturersService;
            this.eventAggregator = eventAggregator;
            this.viewService = viewService;
            eventAggregator.GetEvent<EditStoreItemEvent>().Subscribe(SetProperties);
            this.store = store;
        }

        /// <summary>
        /// Устанавливает свойства при приёме сообщения.
        /// </summary>
        /// <param name="item"></param>
        private async void SetProperties(StoreItem item)
        {
            StoreItem = item;
            Manufacturers = await manufacturersService.GetNamesAsync();
            SelectedManufacturer = Manufacturers.First(name => name == item.Item.Manufacturer.Name);
        }


        #region Command SaveChanges - Команда сохранить изменения товара со склада

        private ICommand? _SaveChangesCommand;
        /// <summary>Команда - сохранить изменения товара со склада</summary>
        public ICommand SaveChangesCommand => _SaveChangesCommand
            ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        private bool CanSaveChangesCommandExecute() => true;
        private async void OnSaveChangesCommandExecuted()
        {
            try
            {
                Manufacturer? existingManufacturer = await manufacturersService.GetAsync(SelectedManufacturer);
                if (existingManufacturer is null)
                {
                    ErrorMessage.Message = "Выбранного производителя нет в каталоге";
                    return;
                }
                StoreItem.Item.ManufacturerId = existingManufacturer.Id;
                await store.EditItemAsync(StoreItem);
                viewService.GetWindow<EditStoreItemWindow>().Close();
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
