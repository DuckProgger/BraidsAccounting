﻿using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels
{
    internal class StoreViewModel : BindableBase
    {
        private IStoreService store;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;
        private CollectionView? collectionView;

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
            eventAggregator.GetEvent<ActionSuccessEvent>().Subscribe(SetStatusMessage);
        }

        /// <summary>
        /// Выбранный материал со склада.
        /// </summary>
        public StoreItem SelectedStoreItem { get; set; } = null!;
        /// <summary>
        /// Коллекция всех материалов на складе.
        /// </summary>
        public ObservableCollection<StoreItem> StoreItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(storeItems);
            }
        }
        /// <summary>
        /// Общее количество материалов на складе.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Выводимое сообщение о статусе.
        /// </summary>
        public MessageProvider StatusMessage { get; } = new(true);

        /// <summary>
        /// Устанавливает статус выполненной операции.
        /// </summary>
        /// <param name="success"></param>
        private void SetStatusMessage(bool success)
        {
            if (success)
            {
                LoadDataCommand.Execute(null);
                StatusMessage.Message = "Операция выполнена";
            }
        }

        #region Command EditItem - Команда редактировать предмет на складе

        private ICommand? _EditItemCommand;
        /// <summary>Команда - редактировать предмет на складе</summary>
        public ICommand EditItemCommand => _EditItemCommand
            ??= new DelegateCommand<string>(OnEditItemCommandExecuted, CanEditItemCommandExecute);
        private bool CanEditItemCommandExecute(string viewName) => true;
        private void OnEditItemCommandExecuted(string viewName)
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
            await store.RemoveItemAsync(SelectedStoreItem.Id);
            storeItems.Remove(SelectedStoreItem);
            MDDialogHost.CloseDialogCommand.Execute(null, null);
            StatusMessage.Message = "Материал удалён со склада";
        }

        #endregion

        #region Command LoadData - Команда загрузки данных со склада

        private ICommand? _LoadDataCommand;
        /// <summary>Команда - загрузки данных со склада</summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
        private bool CanLoadDataCommandExecute() => true;
        private async void OnLoadDataCommandExecuted() => await LoadData();

        private async Task LoadData()
        {
            StatusMessage.Message = "Загружается список материалов на складе...";
            // Нужно обновить контекст, чтобы получать обновлённые данные
            store = ServiceLocator.GetService<IStoreService>();
            StoreItems = new(await store.GetItemsAsync());
            StatusMessage.Message = string.Empty;
            TotalItems = storeItems.Sum(i => i.Count);
        }

        #endregion

        #region Command NavigateToOtherWindow - Команда перейти на другой экран

        private ICommand? _NavigateToOtherWindowCommand;
        private ObservableCollection<StoreItem> storeItems = new();

        /// <summary>Команда - перейти на другой экран</summary>
        public ICommand NavigateToOtherWindowCommand => _NavigateToOtherWindowCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherWindowCommandExecuted, CanNavigateToOtherWindowCommandExecute);
        private bool CanNavigateToOtherWindowCommandExecute(string windowName) => true;
        private void OnNavigateToOtherWindowCommandExecuted(string windowName)
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
    }
}
