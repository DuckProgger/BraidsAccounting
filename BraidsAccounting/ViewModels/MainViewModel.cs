using BraidsAccounting.Models;
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

        public string Title { get; set; } = "Моё окно";
        public MainViewModel()
        {
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

