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
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditStoreItemViewModel : BindableBase
    {
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;
        private readonly IEventAggregator eventAggregator;

        public StoreItem StoreItem { get; set; } = new();
        public ObservableCollection<string> Manufacturers { get; set; }
        public string SelectedManufacturer { get; set; }


        public EditStoreItemViewModel(
         IStoreService store
            ,IManufacturersService manufacturersService
         , IEventAggregator eventAggregator
         )
        {
            this.store = store;
            this.manufacturersService = manufacturersService;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<EditStoreItemEvent>().Subscribe(MessageReceived);
            this.store = store;
        }

        private void MessageReceived(StoreItem item)
        {
            StoreItem = item;
            Manufacturers = new(manufacturersService.GetManufacturerNames());
            SelectedManufacturer = Manufacturers.FirstOrDefault(name => name == item.Item.Manufacturer.Name);
        }


        #region Command SaveChanges - Команда сохранить изменения товара со склада

        private ICommand? _SaveChangesCommand;
        /// <summary>Команда - сохранить изменения товара со склада</summary>
        public ICommand SaveChangesCommand => _SaveChangesCommand
            ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        private bool CanSaveChangesCommandExecute() => true;
        private async void OnSaveChangesCommandExecuted()
        {
            StoreItem.Item.ManufacturerId = manufacturersService.GetManufacturer(SelectedManufacturer).Id;
            store.EditItem(StoreItem);
            eventAggregator.GetEvent<ActionSuccessEvent>().Publish(true);
        }

        #endregion
    }
}
