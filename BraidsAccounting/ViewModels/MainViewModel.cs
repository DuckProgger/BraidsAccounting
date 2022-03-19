﻿using BraidsAccounting.DAL.Entities;
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

namespace BraidsAccounting.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IStoreService storeService;

        //public ObservableCollection<Item>? Items { get; set; }
        //public Item? Item { get; set; } = new();

        //private readonly IRepository<Item> itemsRep;
        //private readonly IRepository<StoreItem> storeRep;
        //private readonly IRepository<Service> servicesRep;
        //private readonly IRepository<WastedItem> wastedItemsRep;
        //private readonly IRepository<ItemPrice> itemPricesRep;

        public string Title { get; set; } = "Моё окно";
        public MainViewModel(
            IStoreService storeService,
            Services.Interfaces.IServiceProvider serviceProvider
           // , IRepository<Service> servicesRep
           //, IRepository<WastedItem> wastedItemsRep
           //, IRepository<ItemPrice> itemPricesRep
            )
        {
            this.storeService = storeService;

            var storeItems = storeService.GetItems().ToArray();

            List <ServiceFormItem> serviceFormItems = new();
            ServiceFormItem item1 = new()
            {
                Count = 8,
                StoreItem = storeItems[0]
            };
            //ServiceFormItem item2 = new()
            //{
            //    Count = 1,
            //    StoreItem = storeItems[1]
            //};
            serviceFormItems.Add(item1);
            //serviceFormItems.Add(item2);   

            serviceProvider.ProvideService(serviceFormItems, "Наташа", 2000);

            //this.itemsRep = itemsRep;
            //this.storeRep = storeRep;
            //this.servicesRep = servicesRep;
            //this.wastedItemsRep = wastedItemsRep;
            //this.itemPricesRep = itemPricesRep;

            //var items = itemsRep.Items.ToArray();
            //var storeItems = storeRep.Items.ToArray();
            //var services = servicesRep.Items.ToArray();
            //var wastedItems = wastedItemsRep.Items.ToArray();
            //var itemPrices = itemPricesRep.Items.ToArray();


        }



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

