using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Cashbox.Visu;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels
{
    internal class StoreViewModel : BindableBase
    {
        private readonly IStoreService store;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;
        private CollectionView collectionView;


        public StoreItem? SelectedStoreItem { get; set; }
        public ObservableCollection<StoreItem?> StoreItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(storeItems);
            }
        }
        public MessageProvider StatusMessage { get; } = new(true);


        public StoreViewModel(
            IStoreService store
            , IEventAggregator eventAggregator
            , IContainerProvider container
            , IRegionManager regionManager
            , IViewService viewService
            )
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
            this.container = container;
            this.regionManager = regionManager;
            this.viewService = viewService;
            eventAggregator.GetEvent<ActionSuccessEvent>().Subscribe(MessageReceived);

        }

        private void MessageReceived(bool success)
        {
            if (success) StatusMessage.Message = "Операция выполнена";
        }

        #region Command EditItem - Команда редактировать предмет на складе

        private ICommand? _EditItemCommand;
        /// <summary>Команда - редактировать предмет на складе</summary>
        public ICommand EditItemCommand => _EditItemCommand
            ??= new DelegateCommand<string>(OnEditItemCommandExecuted, CanEditItemCommandExecute);
        private bool CanEditItemCommandExecute(string viewName) => true;
        private async void OnEditItemCommandExecuted(string viewName)
        {
            OnNavigateToOtherWindowCommandExecuted(viewName);
            eventAggregator.GetEvent<EditStoreItemEvent>().Publish(SelectedStoreItem);
        }

        #endregion

        #region Command RemoveItem - Команда удалить предмет со склада

        private ICommand? _RemoveItemCommand;
        /// <summary>Команда - удалить предмет со склада</summary>
        public ICommand RemoveItemCommand => _RemoveItemCommand
            ??= new DelegateCommand(OnRemoveItemCommandExecuted, CanRemoveItemCommandExecute);
        private bool CanRemoveItemCommandExecute() => true;
        private async void OnRemoveItemCommandExecuted()
        {
            store.RemoveItem(SelectedStoreItem.Id);
            storeItems.Remove(SelectedStoreItem);
            MDDialogHost.CloseDialogCommand.Execute(null, null);
            StatusMessage.Message = "Товар удалён";
        }

        #endregion

        #region Command LoadData - Команда загрузки данных со склада

        private ICommand? _LoadDataCommand;
        /// <summary>Команда - загрузки данных со склада</summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
        private bool CanLoadDataCommandExecute() => true;
        private async void OnLoadDataCommandExecuted()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            StoreItems = new(store.GetItems());
        }

        #endregion

        #region Command NavigateToOtherWindow - Команда перейти на другой экран

        private ICommand? _NavigateToOtherWindowCommand;
        private ObservableCollection<StoreItem?> storeItems = new();

        /// <summary>Команда - перейти на другой экран</summary>
        public ICommand NavigateToOtherWindowCommand => _NavigateToOtherWindowCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherWindowCommandExecuted, CanNavigateToOtherWindowCommandExecute);
        private bool CanNavigateToOtherWindowCommandExecute(string windowName) => true;
        private async void OnNavigateToOtherWindowCommandExecuted(string windowName)
        {
            switch (windowName)
            {
                case nameof(AddStoreItemWindow):
                    viewService.ActivateWindowWithClosing<AddStoreItemWindow, MainWindow>(OnLoadDataCommandExecuted);
                    break;
                case nameof(EditStoreItemWindow):
                    viewService.ActivateWindowWithClosing<EditStoreItemWindow, MainWindow>();
                    break;
                default:
                    break;
            }
        }

        #endregion

        //private void RefreshCollection()
        //{
        //    LoadData();
        //    collectionView.Refresh();
        //}      
    }
}
