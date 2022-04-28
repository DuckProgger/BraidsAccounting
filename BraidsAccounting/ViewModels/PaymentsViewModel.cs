using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
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
    internal class PaymentsViewModel : BindableBase, INotifying
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
        }

        public string Title => "Задолженность";
        public List<Employee> Employees { get; set; }
        public Employee SelectedEmployee { get; set; }
        public Payment NewPayment { get; set; } = new();
        public decimal Debt { get; set; }
        public string DebtStatus { get; set; }
        public bool HaveDebt => Debt > 0;
        public bool NoDebt => Debt <= 0;
        public bool NotSelectedEmployee => string.IsNullOrEmpty(SelectedEmployee?.Name);

        #region Messages

        public Notifier Status { get; } = new(true);        
        public Notifier Error { get; } = new(true);
        public Notifier Warning => throw new NotImplementedException();

        #endregion

        #region Command InitializeData - Команда загрузить начальные данные

        private ICommand? _InitializeDataCommand;
        /// <summary>Команда - загрузить начальные данные</summary>
        public ICommand InitializeDataCommand => _InitializeDataCommand
            ??= new DelegateCommand(OnInitializeDataCommandExecuted, CanInitializeDataCommandExecute);
        private bool CanInitializeDataCommandExecute() => true;
        private async void OnInitializeDataCommandExecuted()
        {
            Employees = await employeesService.GetAllAsync();
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
            NewPayment.Employee = SelectedEmployee;
            await paymentsService.AddAsync(NewPayment);
            NewPayment = new();
            GetDebtCommand.Execute(null);
            Status.Message= "Сумма успешно зачислена.";
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
            if (NewPayment.Amount <= 0)
            {
                Error.Message = "Сумма должна быть положительной.";
                return;
            }
            if (NotSelectedEmployee)
            {
                Error.Message = "Не выбран сотрудник.";
                return;
            }
            MDDialogHost.OpenDialogCommand.Execute(null, null);           
        }

        #endregion
    }
}
