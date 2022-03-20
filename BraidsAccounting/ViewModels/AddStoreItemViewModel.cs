using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Services.Interfaces;
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
    internal class AddStoreItemViewModel : ViewModelBase
    {
        public StoreItem StoreItem { get; set; } /*= new();*/

        public AddStoreItemViewModel(
          IStoreService store
          , IEventAggregator eventAggregator
          )
        {
            this.store = store;
            //StoreItem.Item = new();
            //StoreItem.Item.ItemPrice = new();
            eventAggregator.GetEvent<PubSubEvent<StoreItem>>().Subscribe(MessageReceived);

        }

        private void MessageReceived(StoreItem item)
        {
            StoreItem = item;
            //OnPropertyChanged(nameof(StoreItem));
        }

        #region Command AddStoreItem - Команда добавить товар на склад

        private ICommand? _AddStoreItemCommand;
        private readonly IStoreService store;        

        /// <summary>Команда - добавить товар на склад</summary>
        public ICommand AddStoreItemCommand => _AddStoreItemCommand
            ??= new DelegateCommand(OnAddStoreItemCommandExecuted, CanAddStoreItemCommandExecute);
        private bool CanAddStoreItemCommandExecute() => true;
        private async void OnAddStoreItemCommandExecuted()
        {
            store.AddItem(StoreItem);
        }

        #endregion
    }
}
