using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels
{
    internal class CatalogueViewModel : FilterableBindableBase<Item>, ISignaling
    {
        private ICommand? _NavigateToOtherWindowCommand;
        private readonly IViewService viewService;
        private ICatalogueService catalogueService;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        public CatalogueViewModel(
            IViewService viewService
            , ICatalogueService catalogueService
            , IEventAggregator eventAggregator
            , IRegionManager regionManager
            )
        {
            this.viewService = viewService;
            this.catalogueService = catalogueService;
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            eventAggregator.GetEvent<ActionSuccessEvent>().Subscribe(SetStatusMessage);
        }

        public Item SelectedItem { get; set; }

        #region Messages
        public MessageProvider StatusMessage { get; } = new(true);
        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider WarningMessage => throw new NotImplementedException();

        #endregion

        protected override bool Filter(object obj) => true;
        private void SetStatusMessage(bool success)
        {
            if (regionManager.IsViewActive<CatalogueView>(RegionNames.Catalog) && success)
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
            eventAggregator.GetEvent<EditItemEvent>().Publish(SelectedItem);
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
            StatusMessage.Message = "Загружается каталог материалов...";
            // Нужно обновить контекст, чтобы получать обновлённые данные
            catalogueService = ServiceLocator.GetService<ICatalogueService>();
            Collection = new(await catalogueService.GetAllAsync(false));
            StatusMessage.Message = string.Empty;
        }

        #endregion

        #region Command NavigateToOtherWindow - Команда перейти на другой экран       

        /// <summary>Команда - перейти на другой экран</summary>
        public ICommand NavigateToOtherWindowCommand => _NavigateToOtherWindowCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherWindowCommandExecuted, CanNavigateToOtherWindowCommandExecute);

        private bool CanNavigateToOtherWindowCommandExecute(string windowName) => true;
        private void OnNavigateToOtherWindowCommandExecuted(string windowName)
        {
            switch (windowName)
            {
                case nameof(AddItemWindow):
                    viewService.ActivateWindowWithClosing<AddItemWindow, MainWindow>(OnLoadDataCommandExecuted);
                    break;
                case nameof(EditItemWindow):
                    viewService.ActivateWindowWithClosing<EditItemWindow, MainWindow>();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Command RemoveItem - Команда удалить материал из каталога

        private ICommand? _RemoveItemCommand;
        /// <summary>Команда - удалить материал из каталога</summary>
        public ICommand RemoveItemCommand => _RemoveItemCommand
            ??= new DelegateCommand(OnRemoveItemCommandExecuted, CanRemoveItemCommandExecute);
        private bool CanRemoveItemCommandExecute() => true;
        private async void OnRemoveItemCommandExecuted()
        {
            try
            {
                await catalogueService.RemoveAsync(SelectedItem);
                Collection.Remove(SelectedItem);
                StatusMessage.Message = "Материал успешно удалён из каталога.";
            }
            catch (ArgumentException ex)
            {
                ErrorMessage.Message = ex.Message;
            }
            MDDialogHost.CloseDialogCommand.Execute(null, null);
        }

        #endregion

    }
}
