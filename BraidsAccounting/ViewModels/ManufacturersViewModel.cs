using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace BraidsAccounting.ViewModels;

internal class ManufacturersViewModel : ViewModelBase<Manufacturer>
{
    private readonly IManufacturersService manufacturersService;

    public ManufacturersViewModel(IManufacturersService manufacturersService)
    {
        this.manufacturersService = manufacturersService;
        Title = "Производители";
    }

    /// <summary>
    /// Выбранный производитель в представлении.
    /// </summary>
    public Manufacturer SelectedManufacturer { get; set; } = null!;
    /// <summary>
    /// Отображаемый в представлении прозводитель.
    /// </summary>
    public Manufacturer ManufacturerInForm { get; set; } = new();

    /// <summary>
    /// Список производителей в представлении.
    /// </summary>
    public ObservableCollection<string> ManufacturerList { get; set; } = null!;

    private static bool IsValidManufacturer(Manufacturer manufacturer) =>
       !string.IsNullOrEmpty(manufacturer.Name) &&
        manufacturer.Price >= 0;


    #region Command GetManufacturersList - Команда получить всех производителей

    private ICommand? _GetManufacturersListCommand;
    /// <summary>Команда - получить всех производителей</summary>
    public ICommand GetManufacturersListCommand => _GetManufacturersListCommand
        ??= new DelegateCommand(OnGetManufacturersListCommandExecuted, CanGetManufacturersListCommandExecute);
    private bool CanGetManufacturersListCommandExecute() => true;
    private async void OnGetManufacturersListCommandExecuted()
    {
        Collection = new(await manufacturersService.GetAllAsync());
        ManufacturerList = new(await manufacturersService.GetNamesAsync());
    }

    #endregion

    #region Command Save - Команда сохранить изменения

    private ICommand? _SaveCommand;
    /// <summary>Команда - сохранить изменения</summary>
    public ICommand SaveCommand => _SaveCommand
        ??= new DelegateCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
    private bool CanSaveCommandExecute() => true;
    private async void OnSaveCommandExecuted()
    {
        if (!IsValidManufacturer(ManufacturerInForm))
        {
            Notifier.AddError(Messages.FieldsNotFilled);
            return;
        }
        try
        {
            switch (ManufacturerInForm.Id)
            {
                case 0:
                    await manufacturersService.AddAsync(ManufacturerInForm);
                    Collection.Add(ManufacturerInForm);
                    Notifier.AddInfo(Messages.AddManufacturerSuccess);
                    break;
                default:

                    await manufacturersService.EditAsync(ManufacturerInForm);
                    Notifier.AddInfo(Messages.EditManufacturerSuccess);
                    break;
            }
            GetManufacturersListCommand.Execute(null);
            ResetFormCommand.Execute(null);
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }       
    }

    #endregion

    #region Command RemoveManufacturer - Команда удалить производителя с ценой

    private ICommand? _RemoveManufacturerCommand;
    /// <summary>Команда - удалить производителя с ценой</summary>
    public ICommand RemoveManufacturerCommand => _RemoveManufacturerCommand
        ??= new DelegateCommand(OnRemoveManufacturerCommandExecuted, CanRemoveManufacturerCommandExecute);
    private bool CanRemoveManufacturerCommandExecute() => true;
    private async void OnRemoveManufacturerCommandExecuted()
    {
        try
        {
            await manufacturersService.RemoveAsync(SelectedManufacturer.Id);
            Collection.Remove(SelectedManufacturer);
            Notifier.AddInfo(Messages.RemoveManufacturerSuccess);
            MDDialogHost.CloseDialogCommand.Execute(null, null);
        }
        catch (ArgumentException ex)
        {
            Notifier.AddError(ex.Message);
        }
    }

    #endregion

    #region Command ResetForm - Команда сбросить форму

    private ICommand? _ResetFormCommand;
    /// <summary>Команда - сбросить форму</summary>
    public ICommand ResetFormCommand => _ResetFormCommand
        ??= new DelegateCommand(OnResetFormCommandExecuted, CanResetFormCommandExecute);
    private bool CanResetFormCommandExecute() => true;
    private void OnResetFormCommandExecuted() => ManufacturerInForm = new();

    #endregion

    #region Command FillForm - Команда заполнить форму выбранным производителем для редактирования

    private ICommand? _FillFormCommand;
    /// <summary>Команда - заполнить форму выбранным производителем для редактирования</summary>
    public ICommand FillFormCommand => _FillFormCommand
        ??= new DelegateCommand(OnFillFormCommandExecuted, CanFillFormCommandExecute);
    private bool CanFillFormCommandExecute() => true;
    private void OnFillFormCommandExecuted()
    {
        ManufacturerInForm = new()
        {
            Id = SelectedManufacturer.Id,
            Name = SelectedManufacturer.Name,
            Price = SelectedManufacturer.Price
        };
    }

    #endregion

}

