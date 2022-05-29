using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Input;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace BraidsAccounting.ViewModels;

internal class PaymentsViewModel : ViewModelBase
{
    private readonly IPaymentsService paymentsService;
    private readonly IEmployeesService employeesService;

    public PaymentsViewModel(
        IPaymentsService paymentsService
        , IEmployeesService employeesService
        )
    {
        this.paymentsService = paymentsService;
        this.employeesService = employeesService;
        Title = "Задолженность";
    }

    public List<Employee> Employees { get; set; }
    public Employee SelectedEmployee { get; set; }
    public Payment NewPayment { get; set; } = new();
    public decimal Amount { get; set; }
    public decimal Debt { get; set; }
    public string DebtStatus { get; set; }
    public bool HaveDebt => Debt > 0;
    public bool NoDebt => Debt <= 0;
    public bool NotSelectedEmployee => string.IsNullOrEmpty(SelectedEmployee?.Name);

    #region Command InitializeData - Команда загрузить начальные данные

    private ICommand? _InitializeDataCommand;
    /// <summary>Команда - загрузить начальные данные</summary>
    public ICommand InitializeDataCommand => _InitializeDataCommand
        ??= new DelegateCommand(OnInitializeDataCommandExecuted, CanInitializeDataCommandExecute);
    private bool CanInitializeDataCommandExecute() => true;
    private async void OnInitializeDataCommandExecuted()
    {
        Employees = await employeesService.GetAllAsync();
        if (SelectedEmployee is not null)
            GetDebtCommand.Execute(null);
    }

    #endregion

    #region Command AddPayment - Команда добавить платёж для сотрудника

    private ICommand? _AddPaymentCommand;
    /// <summary>Команда - добавить платёж для сотрудника</summary>
    public ICommand AddPaymentCommand => _AddPaymentCommand
        ??= new DelegateCommand(OnAddPaymentCommandExecuted, CanAddPaymentCommandExecute);
    private bool CanAddPaymentCommandExecute() => true;
    private async void OnAddPaymentCommandExecuted()
    {
        await paymentsService.AddAsync(NewPayment);
        NewPayment = new();
        Amount = 0;
        GetDebtCommand.Execute(null);
        Notifier.AddInfo(Resources.AddPaymentSuccess);
        MDDialogHost.CloseDialogCommand.Execute(null, null);
    }

    #endregion

    #region Command GetDebt - Команда получить задолженность для выбранного сотрудника

    private ICommand? _GetDebtCommand;
    /// <summary>Команда - получить задолженность для выбранного сотрудника</summary>
    public ICommand GetDebtCommand => _GetDebtCommand
        ??= new DelegateCommand(OnGetDebtCommandExecuted, CanGetDebtCommandExecute);
    private bool CanGetDebtCommandExecute() => true;
    private async void OnGetDebtCommandExecuted()
    {
        if (SelectedEmployee is null || NotSelectedEmployee) return;
        Debt = await paymentsService.GetDebtAsync(SelectedEmployee.Name);
        DebtStatus = Debt switch
        {
            > 0 => "Задолженность: ",
            < 0 => "Кредит: ",
            _ => "Задолженность отсутствует",
        };
    }

    #endregion

    #region Command OpenDialog - Команда открыть диалог

    private ICommand? _OpenDialogCommand;
    /// <summary>Команда - открыть диалог</summary>
    public ICommand OpenDialogCommand => _OpenDialogCommand
        ??= new DelegateCommand(OnOpenDialogCommandExecuted, CanOpenDialogCommandExecute);

    private bool CanOpenDialogCommandExecute() => true;
    private void OnOpenDialogCommandExecuted()
    {
        NewPayment.Employee = SelectedEmployee;
        NewPayment.Amount = Amount;
        if (!paymentsService.Validate(NewPayment, out IEnumerable<string> errorMessages))
        {
            foreach (var errorMessage in errorMessages)
                Notifier.AddError(errorMessage);
            return;
        }
        MDDialogHost.OpenDialogCommand.Execute(null, null);
    }

    #endregion
}
