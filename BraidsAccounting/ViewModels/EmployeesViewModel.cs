using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Exceptions;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class EmployeesViewModel : ViewModelBase<Employee>
{
    private readonly IEmployeesService employeesService;

    public EmployeesViewModel(IEmployeesService employeesService)
    {
        this.employeesService = employeesService;
        Title = "Сотрудники";    
    }

    public Employee SelectedEmployee { get; set; }
    public Employee EmployeeInForm { get; set; } = new();

    /// <summary>
    /// Список сотрудников в представлении.
    /// </summary>
    public ObservableCollection<string> EmployeeList { get; set; } = null!;    

    #region Command GetEmployees - Команда получить всех сотрудников.

    private ICommand? _GetEmployeesCommand;
    /// <summary>Команда - получить всех сотрудников.</summary>
    public ICommand GetEmployeesCommand => _GetEmployeesCommand
        ??= new DelegateCommand(OnGetEmployeesCommandExecuted, CanGetEmployeesCommandExecute);
    private bool CanGetEmployeesCommandExecute() => true;
    private async void OnGetEmployeesCommandExecuted()
    {
        Collection = new(await employeesService.GetAllAsync());
        EmployeeList = new(Collection.Select(e => e.Name));
    }

    #endregion

    #region Command FillForm - Команда заполнить форму выбранным производителем для редактирования

    private ICommand? _FillFormCommand;
    /// <summary>Команда - заполнить форму выбранным производителем для редактирования</summary>
    public ICommand FillFormCommand => _FillFormCommand
        ??= new DelegateCommand(OnFillFormCommandExecuted, CanFillFormCommandExecute);
    private bool CanFillFormCommandExecute() => true;
    private void OnFillFormCommandExecuted()
    {
        EmployeeInForm = new()
        {
            Id = SelectedEmployee.Id,
            Name = SelectedEmployee.Name,
        };
    }

    #endregion

    #region Command ResetForm - Команда сбросить форму

    private ICommand? _ResetFormCommand;
    /// <summary>Команда - сбросить форму</summary>
    public ICommand ResetFormCommand => _ResetFormCommand
        ??= new DelegateCommand(OnResetFormCommandExecuted, CanResetFormCommandExecute);
    private bool CanResetFormCommandExecute() => true;
    private void OnResetFormCommandExecuted() => EmployeeInForm = new();

    #endregion

    #region Command Save - Команда сохранить изменения

    private ICommand? _SaveCommand;
    /// <summary>Команда - сохранить изменения</summary>
    public ICommand SaveCommand => _SaveCommand
        ??= new DelegateCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
    private bool CanSaveCommandExecute() => true;
    private async void OnSaveCommandExecuted()
    {
        if (!employeesService.Validate(EmployeeInForm, out IEnumerable<string> errorMessages))
        {
            foreach (var errorMessage in errorMessages)
                Notifier.AddError(errorMessage);
            return;
        }
        try
        {
            switch (EmployeeInForm.Id)
            {
                case 0:
                    await employeesService.AddAsync(EmployeeInForm);
                    Collection.Add(EmployeeInForm);
                    Notifier.AddInfo(Resources.AddEmployeeSuccess);
                    break;
                default:

                    await employeesService.EditAsync(EmployeeInForm);
                    Notifier.AddInfo(Resources.EditEmployeeSuccess);
                    break;
            }
            GetEmployeesCommand.Execute(null);
            ResetFormCommand.Execute(null);
        }
        catch (DublicateException ex)
        {
            Notifier.AddError(ex.Message);
        }       
    }

    #endregion

}
