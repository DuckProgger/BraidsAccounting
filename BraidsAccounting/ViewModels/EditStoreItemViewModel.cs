using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditStoreItemViewModel : BindableBase
    {
        private readonly IStoreService store;

        public StoreItem StoreItem { get; set; } = new();

        public EditStoreItemViewModel(
         IStoreService store
         , IEventAggregator eventAggregator
         )
        {
            this.store = store;
            eventAggregator.GetEvent<EditStoreItemEvent>().Subscribe(MessageReceived);
            this.store = store;
        }

        private void MessageReceived(StoreItem item)
        {
            StoreItem = item;
        }


        #region Command SaveChanges - Команда сохранить изменения товара со склада

        private ICommand? _SaveChangesCommand;
        /// <summary>Команда - сохранить изменения товара со склада</summary>
        public ICommand SaveChangesCommand => _SaveChangesCommand
            ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        private bool CanSaveChangesCommandExecute() => true;
        private async void OnSaveChangesCommandExecuted()
        {
            store.EditItem(StoreItem);
        }

        #endregion
    }
}
