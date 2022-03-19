using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IServiceProvider = BraidsAccounting.Services.Interfaces.IServiceProvider;

namespace BraidsAccounting.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {

        //public ObservableCollection<Item>? Items { get; set; }
        //public Item? Item { get; set; } = new();

        private readonly IRepository<Item> itemsRep;
        private readonly IStoreService store;
        private readonly IServiceProvider servicesRep;
        private readonly IRepository<WastedItem> wastedItemsRep;
        private readonly IRepository<ItemPrice> itemPricesRep;

        public string Title { get; set; } = "Моё окно";
        //public MainViewModel(
        //    IStoreService storeService,
        //    Services.Interfaces.IServiceProvider serviceProvider
        //   // , IRepository<Service> servicesRep
        //   //, IRepository<WastedItem> wastedItemsRep
        //   //, IRepository<ItemPrice> itemPricesRep
        //    )
        //{


        //}

        public ViewModel CurrentModel { get; set; }

        #region Command LoadStoreView - Команда загрузить StoreView

        private ICommand? _LoadStoreViewCommand;

        public MainViewModel(
            IRepository<Item> itemsRep
            , IStoreService store
            , IServiceProvider servicesRep
            , IRepository<WastedItem> wastedItemsRep
            , IRepository<ItemPrice> itemPricesRep
            )
        {
            this.itemsRep = itemsRep;
            this.store = store;
            this.servicesRep = servicesRep;
            this.wastedItemsRep = wastedItemsRep;
            this.itemPricesRep = itemPricesRep;
        }

        /// <summary>Команда - загрузить StoreViewModel</summary>
        public ICommand LoadStoreViewCommand => _LoadStoreViewCommand
            ??= new DelegateCommand(OnLoadStoreViewModelCommandExecuted, CanLoadStoreViewModelCommandExecute);
        private bool CanLoadStoreViewModelCommandExecute() => true;
        private async void OnLoadStoreViewModelCommandExecuted()
        {
            CurrentModel = new StoreViewModel(store);
        }

        #endregion


        //private ICommand? _CreateItemCommand;
        //public ICommand? CreateItemCommand => _CreateItemCommand ??= new DelegateCommand<Item>(async (item) => Item = await ItemRepo.Create(item));

        //private ICommand? _getItemsCommand;
        //public ICommand? GetItemsCommand => _getItemsCommand ??= new DelegateCommand(async () => Items = new(await ItemRepo.GetAll()));

        //private ICommand? _getItemCommand;
        //public ICommand? GetItemCommand => _getItemCommand ??= new DelegateCommand<int?>(async (id) => Item = await ItemRepo.Get(id));

        //private ICommand? _editItemCommand;
        //public ICommand? EditItemCommand => _editItemCommand ??= new DelegateCommand<Item>(async (item) => 
        //{
        //    Item = await ItemRepo.Edit(item);
        //    oncol
        //});

        //private ICommand? _deleteItemCommand;
        //public ICommand? DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand<int?>(async (id) => Item = await ItemRepo.Delete(id));


    }

}

