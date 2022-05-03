using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class EditItemViewModel : ViewModelBase
{
    private ICommand? _SaveChangesCommand;
    private readonly ICatalogueService catalogueService;
    private readonly IViewService viewService;
    private readonly IManufacturersService manufacturersService;

    public EditItemViewModel(
        ICatalogueService catalogueService
        , IViewService viewService
        , IManufacturersService manufacturersService
        )
    {
        this.catalogueService = catalogueService;
        this.viewService = viewService;
        this.manufacturersService = manufacturersService;
    }

    /// <summary>
    /// Материал из каталога, обрабатываемый в форме.
    /// </summary>
    public Item ItemInForm { get; set; } = null!;
    /// <summary>
    /// Список производителей.
    /// </summary>
    public List<Manufacturer>? Manufacturers { get; set; }
    /// <summary>
    /// Выбранный производитель из списка.
    /// </summary>
    public Manufacturer SelectedManufacturer { get; set; } = null!;

    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
        Item? item = navigationContext.Parameters[ParameterNames.SelectedItem] as Item;
        if (item is not null)
        {
            SelectedManufacturer = item.Manufacturer;
            ItemInForm = item;
        }
    }
    private bool IsValidItem() =>
        !string.IsNullOrEmpty(SelectedManufacturer?.Name)
       && !string.IsNullOrEmpty(ItemInForm.Color)
       && !string.IsNullOrEmpty(ItemInForm.Article);

    #region Command SaveChanges - Команда сохранить изменения товара со склада

    /// <summary>Команда - сохранить изменения товара со склада</summary>
    public ICommand SaveChangesCommand => _SaveChangesCommand
        ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);

    private bool CanSaveChangesCommandExecute() => true;
    private async void OnSaveChangesCommandExecuted()
    {
        try
        {
            if (!IsValidItem())
            {
                Notifier.AddError(MessageContainer.FieldsNotFilled);
                return;
            }
            ItemInForm.Manufacturer = SelectedManufacturer;
            await catalogueService.EditAsync(ItemInForm);
            viewService.AddParameter(ParameterNames.EditItemResult, true);
            viewService.GoBack();
        }
        catch (ArgumentException)
        {
            Notifier.AddError(MessageContainer.FieldsNotFilled);
        }
    }

    #endregion
}
