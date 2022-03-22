using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using IServiceProvider = BraidsAccounting.Services.Interfaces.IServiceProvider;

namespace BraidsAccounting.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {

        //public ObservableCollection<Item>? Items { get; set; }
        //public Item? Item { get; set; } = new();

        private readonly IRepository<Item> itemsRep;
        private readonly IStoreService store;
        private readonly IServiceProvider servicesRep;
        private readonly IRepository<WastedItem> wastedItemsRep;
        private readonly IRepository<ItemPrice> itemPricesRep;
        private readonly IContainerExtension container;
        private readonly IRegionManager regionManager;

        public string Title { get; set; } = "Моё окно"; 

        private ICommand? _LoadStoreViewCommand;

        public MainWindowViewModel(
            IRepository<Item> itemsRep
            , IStoreService store
            , IServiceProvider servicesRep
            , IRepository<WastedItem> wastedItemsRep
            , IRepository<ItemPrice> itemPricesRep
            , IContainerExtension container
            , IRegionManager regionManager
            )
        {
            this.itemsRep = itemsRep;
            this.store = store;
            this.servicesRep = servicesRep;
            this.wastedItemsRep = wastedItemsRep;
            this.itemPricesRep = itemPricesRep;
            this.container = container;
            this.regionManager = regionManager;
        }

        #region Command LoadStoreView - Команда загрузить StoreView

        /// <summary>Команда - загрузить StoreViewModel</summary>
        public ICommand LoadStoreViewCommand => _LoadStoreViewCommand
            ??= new DelegateCommand(OnLoadStoreViewModelCommandExecuted, CanLoadStoreViewModelCommandExecute);
        private bool CanLoadStoreViewModelCommandExecute() => true;
        private async void OnLoadStoreViewModelCommandExecuted()
        {
            //CurrentModel = new StoreViewModel(store);
        }

        #endregion


    
        #region Command NavigateToOtherView - Команда переключиться на другое представление

        private ICommand? _NavigateToOtherViewCommand;
        /// <summary>Команда - переключиться на другое представление</summary>
        public ICommand NavigateToOtherViewCommand => _NavigateToOtherViewCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherViewCommandExecuted, CanNavigateToOtherViewCommandExecute);
        private bool CanNavigateToOtherViewCommandExecute(string viewName) => true;
        private async void OnNavigateToOtherViewCommandExecuted(string viewName)
        {
            regionManager.RequestNavigate("ContentRegion", viewName);

        }

        #endregion


    }

}

