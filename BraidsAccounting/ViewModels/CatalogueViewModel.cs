using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public CatalogueViewModel(
            IViewService viewService,
            ICatalogueService catalogueService
            )
        {
            this.viewService = viewService;
            this.catalogueService = catalogueService;
        }

        public Item SelectedItem { get; set; }

        #region Messages
        public MessageProvider StatusMessage { get; } = new(true);
        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider WarningMessage => throw new NotImplementedException();

        #endregion

        protected override bool Filter(object obj) => true;

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
