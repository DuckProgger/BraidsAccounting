using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
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
        //public ObservableCollection<Item>? Items { get; set; }
        //public Item? Item { get; set; } = new();

        private readonly IRepository<Item> itemsRep;
        private readonly IRepository<StoreItem> storeRep;
        private readonly IRepository<Service> servicesRep;

        public string Title { get; set; } = "Моё окно";
        public MainViewModel(IRepository<Item> itemsRep, IRepository<StoreItem> storeRep, IRepository<Service> servicesRep)
        {
            this.itemsRep = itemsRep;
            this.storeRep = storeRep;
            this.servicesRep = servicesRep;

            //var items = itemsRep.Items.Take(1).ToArray();
            //var storeItems = storeRep.Items.Take(1).ToArray();
            //var services = servicesRep.Items.Take(1).ToArray();

            //Service service = new()
            //{
            //    Items = new()
            //    {
            //        new() { Item = itemsRep.Get(1), Count = 2, Price = 100 },
            //        new() { Item = itemsRep.Get(3), Count = 2, Price = 100 }
            //    },
            //    Profit = 1000,
            //    NetProfit = 800
            //};
            //servicesRep.Create(service);

            //Service service = servicesRep.Get(6);
            //foreach (var wastedItem in service.WastedItems)
            //{
            //    var i = storeRep.Items.First(si => si.EnumerableItem.Item.Id == wastedItem.Item.Id);
            //    i.EnumerableItem.Count -= wastedItem.Count;
            //    if (i.EnumerableItem.Count > 0)
            //        storeRep.Edit(i);
            //    else if (i.EnumerableItem.Count == 0)
            //        storeRep.Delete(1);
            //    else
            //        throw new Exception("Отсутсвует требуемое количество материала на складе.");
            //}

            //var dfg = storeRep.Items.ToArray();


            StoreItem si = new()
            {
                EnumerableItem = new()
                {
                    Item = new()
                    {
                        Manufacturer = "easy",
                        Article = "model 6",
                        Color = "purple"
                    },
                    Count = 10,
                    Price = 150
                }
            };

            AddStoreItem(si);

        }

        private void AddStoreItem(StoreItem storeItem)
        {
            var existingItem = ((IEnumerable<Item>)itemsRep.Items)
                .FirstOrDefault(i => i.Equals(storeItem.EnumerableItem.Item));
            // Продукта в каталоге нет - надо добавить
            if (existingItem is null)
            {
                var item = itemsRep.Create(storeItem.EnumerableItem.Item);
                storeItem.EnumerableItem.Item = item;
                storeRep.Create(storeItem);
                return;
            }
            var existingStoreItem = ((IEnumerable<StoreItem>)storeRep.Items)
                .FirstOrDefault(si => si.EnumerableItem.Equals(storeItem.EnumerableItem));
            // Продукт в каталоге есть, но нет на складе - добавить на склад
            if (existingStoreItem is null)
            {
                storeItem.EnumerableItem.Item = existingItem;
                storeRep.Create(storeItem);
                return;
            }
            // Продукт есть в каталоге и на складе - изменить количество на складе
            existingStoreItem.EnumerableItem.Count += storeItem.EnumerableItem.Count;
            storeRep.Edit(existingStoreItem);
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

