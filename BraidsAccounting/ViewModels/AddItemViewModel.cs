using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class AddItemViewModel : ViewModelBase
{
    private readonly ICatalogueService catalogueService;
    private readonly IViewService viewService;

    public AddItemViewModel(ICatalogueService catalogueService, IViewService viewService)
    {
        this.catalogueService = catalogueService;
        this.viewService = viewService;
        ItemInForm.Manufacturer = new();
        Title = "Добавление материала в каталог";
    }
    /// <summary>
    /// Материал со склада, обрабатываемый в форме.
    /// </summary>
    public Item ItemInForm { get; set; } = new();
    /// <summary>
    /// Список производителей.
    /// </summary>
    public List<Manufacturer>? Manufacturers { get; set; }
    /// <summary>
    /// Выбранный производитель из списка.
    /// </summary>
    public Manufacturer SelectedManufacturer { get; set; } = new();

    #region Command AddItem - Команда добавить новый материал в каталог

    private ICommand? _AddItemCommand;
    /// <summary>Команда - добавить новый материал в каталог</summary>
    public ICommand AddItemCommand => _AddItemCommand
        ??= new DelegateCommand(OnAddItemCommandExecuted, CanAddItemCommandExecute);
    private bool CanAddItemCommandExecute() => true;
    private async void OnAddItemCommandExecuted()
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
            await catalogueService.AddAsync(ItemInForm);
            viewService.AddParameter(ParameterNames.AddItemResult, true);
            viewService.GoBack();
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }
    }

    #endregion

    #region Command InitializeData - Команда заполнить форму начальными данными

    private ICommand? _InitializeDataCommand;

    /// <summary>Команда - заполнить форму начальными данными</summary>
    public ICommand InitializeDataCommand => _InitializeDataCommand
        ??= new DelegateCommand(OnInitialDataCommandExecuted, CanInitialDataCommandExecute);

    private bool CanInitialDataCommandExecute() => true;
    private async void OnInitialDataCommandExecuted()
    {
        IManufacturersService? manufacturersService = ServiceLocator.GetService<IManufacturersService>();
        Manufacturers = await manufacturersService.GetAllAsync().ConfigureAwait(false);
    }

    #endregion

}
