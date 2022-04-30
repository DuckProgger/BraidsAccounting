using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class EditStoreItemViewModel : ViewModelBase
    {
        private readonly IStoreService store;
        private readonly IManufacturersService manufacturersService;
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

        public EditStoreItemViewModel(
         IStoreService store
            , IManufacturersService manufacturersService
            , IViewService viewService
         )
        {
            this.store = store;
            this.manufacturersService = manufacturersService;
            this.viewService = viewService;
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
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            StoreItem? item = navigationContext.Parameters["item"] as StoreItem;
            if (item is not null)
                SetProperties(item);
        }       

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
                    Notifier.AddError(MessageContainer.Get("manufacturerNotFound"));
                    return;
                }
                StoreItem.Item.ManufacturerId = existingManufacturer.Id;
                await store.EditItemAsync(StoreItem);
                viewService.AddParameter(ParameterNames.EditItemResult, true);
                viewService.GoBack();
            }
            catch (ArgumentException)
            {
                Notifier.AddError(MessageContainer.Get("fieldsNotFilled"));
            }
        }

        #endregion
    }
}
