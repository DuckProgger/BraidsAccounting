using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
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
    private Manufacturer selectedManufacturer;


    public EditItemViewModel(
        ICatalogueService catalogueService
        , IViewService viewService
        , IManufacturersService manufacturersService
        )
    {
        this.catalogueService = catalogueService;
        this.viewService = viewService;
        this.manufacturersService = manufacturersService;
        Title = "Изменение материала в каталоге";
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
    public Manufacturer SelectedManufacturer
    {
        get => selectedManufacturer;
        set => selectedManufacturer = value;
    }

    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
        Item? item = navigationContext.Parameters[ParameterNames.SelectedItem] as Item;
        if (item is not null)
        {
            SelectedManufacturer = Manufacturers.Find(m => m.Name.Equals(item.Manufacturer.Name));
            ItemInForm = item;
        }
    }

    #region Command SaveChanges - Команда сохранить изменения товара со склада

    /// <summary>Команда - сохранить изменения товара со склада</summary>
    public ICommand SaveChangesCommand => _SaveChangesCommand
        ??= new DelegateCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);

    private bool CanSaveChangesCommandExecute() => true;
    private async void OnSaveChangesCommandExecuted()
    {
        ItemInForm.Manufacturer = SelectedManufacturer;
        if (!catalogueService.Validate(ItemInForm, out IEnumerable<string> errorMessages))
        {
            foreach (var errorMessage in errorMessages)
                Notifier.AddError(errorMessage);
            return;
        }
        try
        {            
            await catalogueService.EditAsync(ItemInForm);
            viewService.AddParameter(ParameterNames.EditItemResult, true);
            viewService.GoBack();
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }
        catch (Exception ex)
        {
            Notifier.AddError(ex.Message);
        }
    }

    #endregion
}
