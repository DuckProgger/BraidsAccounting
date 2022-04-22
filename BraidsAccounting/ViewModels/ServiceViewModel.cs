using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Events;
using BraidsAccounting.Models;
using BraidsAccounting.Modules;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.ViewModels.Interfaces;
using BraidsAccounting.Views;
using BraidsAccounting.Views.Windows;
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
using System.Windows;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace BraidsAccounting.ViewModels
{
    internal class ServiceViewModel : BindableBase, ISignaling
    {
        private readonly Services.Interfaces.IServiceProvider serviceProvider;
        private readonly IRegionManager regionManager;
        private readonly IViewService viewService;
        private const int stockWarningThreshold = 2;

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
            eventAggregator.GetEvent<SelectItemEvent>().Subscribe(AddWastedItemToService);
        }

        /// <summary>
        /// Выполненная работа, заполненная в представлении
        /// </summary>
        public Service Service { get; set; } = new();
        /// <summary>
        /// Список израсходованных материалов выполненной работы.
        /// </summary>
        public ObservableCollection<FormItem> WastedItems { get; set; } = new();
        /// <summary>
        /// Выбранный материал в представлении, который будет добавлен
        /// в список израсходованных материалов.
        /// </summary>
        public FormItem SelectedWastedItem { get; set; } = new();
        /// <summary>
        /// Список имён сотрудников, которые когда-либо выполняли работу.
        /// </summary>
        public List<string>? Names { get; set; }
        /// <summary>
        /// Выбранный сотрудник, оказавший услугу.
        /// </summary>
        public string SelectedEmployee { get; set; }

        #region Messages

        public MessageProvider StatusMessage { get; } = new(true);
        
        public MessageProvider ErrorMessage { get; } = new(true);
        
        public MessageProvider WarningMessage { get; } = new();

        #endregion

        /// <summary>
        /// Добавляет выбранный материал в коллекцию израсходованных
        /// материалов выполненной работы
        /// </summary>
        /// <param name="catalogueItem"></param>
        private void AddWastedItemToService(Item? catalogueItem)
        {
            if (catalogueItem is not null && regionManager.IsViewActive<ServiceView>(RegionNames.Main))
            {
                if (WastedItems.FirstOrDefault(wi => wi.Equals(catalogueItem)) != null)
                {
                    ErrorMessage.Message = "Выбранный материал уже есть в списке";
                    return;
                }
                FormItem formItem = catalogueItem;
                if (formItem.MaxCount == 0)
                {
                    ErrorMessage.Message = "Выбранный материал отсутсвует на складе";
                    return;
                }
                WastedItems.Add(formItem);
            }
        }
        private void CheckRunningOutItems(IEnumerable<FormItem> wastedItems)
        {
            var runningOutItems = wastedItems.Where(i => i.MaxCount - i.Count <= stockWarningThreshold).ToArray();
            if (runningOutItems.Length > 0)
            {
                StringBuilder sb = new();
                sb.Append("Заканчиваются следующие материалы:");
                sb.Append(Environment.NewLine);
                foreach (var item in runningOutItems)
                {
                    sb.Append($"{item.Manufacturer} {item.Article} {item.Color} осталось {item.MaxCount - item.Count} шт.");
                    sb.Append(Environment.NewLine);
                }
                MessageBox.Show(sb.ToString(), "ВНИМАНИЕ!");
            }
        }

        #region Command CreateService - Добавление сервиса

        private ICommand? _CreateServiceCommand;
        /// <summary>Команда - Добавление сервиса</summary>
        public ICommand CreateServiceCommand => _CreateServiceCommand
            ??= new DelegateCommand(OnCreateServiceCommandExecuted, CanCreateServiceCommandExecute);
        private bool CanCreateServiceCommandExecute() => true;
        private async void OnCreateServiceCommandExecuted()
        {
            MDDialogHost.CloseDialogCommand.Execute(null, null);
            Service.WastedItems = new();
            foreach (FormItem? item in WastedItems)
                Service.WastedItems.Add(item);
            Service.Employee = new();
            Service.Employee.Name = SelectedEmployee;
            try
            {
                await serviceProvider.AddAsync(Service);
                CheckRunningOutItems(WastedItems);
                Service = new();
                WastedItems = new();
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
        private void OnSelectStoreItemCommandExecuted() => viewService.ActivateWindowWithClosing<ItemsCatalogueWindow, MainWindow>();

        #endregion

        #region Command OpenDialog - Команда открыть диалог

        private ICommand? _OpenDialogCommand;
        /// <summary>Команда - открыть диалог</summary>
        public ICommand OpenDialogCommand => _OpenDialogCommand
            ??= new DelegateCommand(OnOpenDialogCommandExecuted, CanOpenDialogCommandExecute);
        private bool CanOpenDialogCommandExecute() => true;
        private void OnOpenDialogCommandExecuted()
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
        private void OnRemoveWastedItemCommandExecuted() => WastedItems.Remove(SelectedWastedItem);

        #endregion

        #region Command InitializeData - Команда заполнить форму начальными данными

        private ICommand? _InitializeDataCommand;
        /// <summary>Команда - заполнить форму начальными данными</summary>
        public ICommand InitializeDataCommand => _InitializeDataCommand
            ??= new DelegateCommand(OnInitialDataCommandExecuted, CanInitialDataCommandExecute);
        private bool CanInitialDataCommandExecute() => true;
        private async void OnInitialDataCommandExecuted()
        {
            var employeesService = ServiceLocator.GetService<IEmployeesService>();
            var employees = await employeesService.GetAllAsync();
            Names = new(employees.Select(e => e.Name));
        }

        #endregion
    }
}
