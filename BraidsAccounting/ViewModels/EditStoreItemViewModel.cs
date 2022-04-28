using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using BraidsAccounting.Views.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditStoreItemViewModel : BindableBase, INotifying, INavigationAware
    {
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewService viewService;

        /// <summary>
        /// Материал со склада, обрабатываемый в форме.
        /// </summary>
        public StoreItem StoreItem { get; set; } = new();
        /// <summary>
        /// Список производителей.
        /// </summary>
        public List<string>? Manufacturers { get; set; }
        /// <summary>
        /// Выбранный производитель из списка.
        /// </summary>
        public string SelectedManufacturer { get; set; } = null!;

        #region Messages

        public Notifier Error { get; } = new(true);
        public Notifier Status => throw new NotImplementedException();
        public Notifier Warning => throw new NotImplementedException();

        #endregion


        public EditStoreItemViewModel(
         IStoreService store
            , IManufacturersService manufacturersService
            , IEventAggregator eventAggregator
            , IViewService viewService
         )
        {
            this.store = store;
            this.manufacturersService = manufacturersService;
            this.eventAggregator = eventAggregator;
            this.viewService = viewService;
            //eventAggregator.GetEvent<EditStoreItemEvent>().Subscribe(SetProperties);
            this.store = store;
        }

        /// <summary>
        /// Устанавливает свойства при приёме сообщения.
        /// </summary>
        /// <param name="item"></param>
        private async void SetProperties(StoreItem item)
        {
            StoreItem = item;
            Manufacturers = await manufacturersService.GetNamesAsync();
            SelectedManufacturer = Manufacturers.First(name => name == item.Item.Manufacturer.Name);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var item = navigationContext.Parameters["item"] as StoreItem;
            if (item is not null)
                SetProperties(item);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }


        #region Command SaveChanges - Команда сохранить изменения товара со склада

        private ICommand? _SaveChangesCommand;
        /// <summary>Команда - сохранить изменения товара со склада</summary>
        public ICommand SaveChangesCommand => _SaveChangesCommand
            ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);      

        private bool CanSaveChangesCommandExecute() => true;
        private async void OnSaveChangesCommandExecuted()
        {
            try
            {
                Manufacturer? existingManufacturer = await manufacturersService.GetAsync(SelectedManufacturer);
                if (existingManufacturer is null)
                {
                    Error.Message = "Выбранного производителя нет в каталоге";
                    return;
                }
                StoreItem.Item.ManufacturerId = existingManufacturer.Id;
                await store.EditItemAsync(StoreItem);
                viewService.AddParameter("result", true);
                viewService.GoBack();
                //viewService.ClosePopupWindow();
                //viewService.GetWindow<EditStoreItemWindow>().Close();
                //eventAggregator.GetEvent<ActionSuccessEvent>().Publish(true);
            }
            catch (ArgumentException)
            {
                Error.Message = "Заполнены не все поля";
            }
        }

        #endregion
    }
}
