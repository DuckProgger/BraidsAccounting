using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Interfaces;
using BraidsAccounting.Models;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
using Cashbox.Visu;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace BraidsAccounting.ViewModels
{
    internal class ServiceViewModel : BindableBase
    {
        public Service Service { get; set; } = new();
        public ObservableCollection<FormItem> WastedItems { get; set; } = new();
        public FormItem SelectedWastedItem { get; set; } = new();
        public MessageProvider StatusMessage { get; } = new(true);
        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider WarningMessage { get; } = new();


        private readonly Services.Interfaces.IServiceProvider serviceProvider;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;

        public ServiceViewModel(
            Services.Interfaces.IServiceProvider serviceProvider
            , IEventAggregator eventAggregator
            , IRegionManager regionManager
            , IViewService viewService
            )
        {
            this.serviceProvider = serviceProvider;
            this.regionManager = regionManager;
            this.viewService = viewService;
            eventAggregator.GetEvent<SelectStoreItemEvent>().Subscribe(MessageReceived);
        }

        private void MessageReceived(StoreItem? storeItem)
        {
            if (storeItem is not null && regionManager.IsViewActive<ServiceView>("ContentRegion"))
            {
                storeItem.Count = 0;
                WastedItems.Add(storeItem);
            }
        }

        #region Command CreateService - Добавление сервиса

        private ICommand _CreateServiceCommand;
        /// <summary>Команда - Добавление сервиса</summary>
        public ICommand CreateServiceCommand => _CreateServiceCommand
            ??= new DelegateCommand(OnCreateServiceCommandExecuted, CanCreateServiceCommandExecute);
        private bool CanCreateServiceCommandExecute() => true;
        private async void OnCreateServiceCommandExecuted()
        {
            MDDialogHost.CloseDialogCommand.Execute(null, null);
            Service.WastedItems = new();
            foreach (var item in WastedItems)
                Service.WastedItems.Add(item);
            try
            {
                serviceProvider.ProvideService(Service);
                StatusMessage.Message = "Новая работа добавлена";
            }
            catch (ArgumentException)
            {
                ErrorMessage.Message = "Не все поля заполнены";
            }
            catch (DbUpdateException)
            {
                ErrorMessage.Message = "Не все поля заполнены";
            }
        }

        #endregion

        #region Command SelectStoreItem - Команда выбрать товар со склада

        private ICommand? _SelectStoreItemCommand;

        /// <summary>Команда - выбрать товар со склада</summary>
        public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
            ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
        private bool CanSelectStoreItemCommandExecute() => true;
        private async void OnSelectStoreItemCommandExecuted()
        {
            viewService.ActivateWindowWithClosing<SelectStoreItemWindow, MainWindow>();
        }

        #endregion

        #region Command OpenDialog - Команда открыть диалог

        private ICommand? _OpenDialogCommand;
        /// <summary>Команда - открыть диалог</summary>
        public ICommand OpenDialogCommand => _OpenDialogCommand
            ??= new DelegateCommand(OnOpenDialogCommandExecuted, CanOpenDialogCommandExecute);
        private bool CanOpenDialogCommandExecute() => true;
        private async void OnOpenDialogCommandExecuted()
        {
            WarningMessage.Message = WastedItems.Count == 0 ? "НЕ ВЫБРАН НИ ОДИН МАТЕРИАЛ!" : string.Empty;
            MDDialogHost.OpenDialogCommand.Execute(null, null);
        }

        #endregion

        #region Command RemoveWastedItem - Команда удалить использованный материал

        private ICommand? _RemoveWastedItemCommand;
        /// <summary>Команда - удалить использованный материал</summary>
        public ICommand RemoveWastedItemCommand => _RemoveWastedItemCommand
            ??= new DelegateCommand(OnRemoveWastedItemCommandExecuted, CanRemoveWastedItemCommandExecute);
        private bool CanRemoveWastedItemCommandExecute() => true;
        private async void OnRemoveWastedItemCommandExecuted()
        {
            WastedItems.Remove(SelectedWastedItem);
        }

        #endregion
    }
}
