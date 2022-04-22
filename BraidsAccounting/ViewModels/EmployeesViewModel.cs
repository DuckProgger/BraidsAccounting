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
using System.Windows.Data;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    class EmployeesViewModel : FilterableBindableBase<Employee>, ISignaling
    {
        private readonly IEmployeesService employeesService;
        private string? employeeFilter;

        public EmployeesViewModel(IEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }
      
        public Employee SelectedEmployee { get; set; }
        public Employee EmployeeInForm { get; set; } = new();

        /// <summary>
        /// Список сотрудников в представлении.
        /// </summary>
        public ObservableCollection<string> EmployeeList { get; set; } = null!;
        /// <summary>
        /// Значение, введённое в поле фильтра производителя.
        /// </summary>
        public string? EmployeeFilter
        {
            get => employeeFilter;
            set
            {
                employeeFilter = value;
                collectionView?.Refresh();
            }
        }

        #region Messages

        public MessageProvider StatusMessage { get; } = new(true);        
        public MessageProvider ErrorMessage { get; } = new(true);
        public MessageProvider WarningMessage { get; } = new();

        #endregion

        protected override bool Filter(object obj)
        {
            Employee item = (Employee)obj;
            bool manufacturerCondition = string.IsNullOrEmpty(EmployeeFilter)
                || item.Name.Contains(EmployeeFilter, StringComparison.OrdinalIgnoreCase);
            return manufacturerCondition;
        }

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

        //#region Command EditEmployee - Команда редактировать существующего сотрудника

        //private ICommand? _EditEmployeeCommand;
        ///// <summary>Команда - редактировать существующего сотрудника</summary>
        //public ICommand EditEmployeeCommand => _EditEmployeeCommand
        //    ??= new DelegateCommand(OnEditEmployeeCommandExecuted, CanEditEmployeeCommandExecute);
        //private bool CanEditEmployeeCommandExecute() => true;
        //private async void OnEditEmployeeCommandExecuted()
        //{
        //    await employeesService.EditEmployeeAsync(EmployeeInForm);
        //}

        //#endregion

        //#region Command AddEmployee - Команда добавить нового сотрудника

        //private ICommand? _AddEmployeeCommand;
        ///// <summary>Команда - добавить нового сотрудника</summary>
        //public ICommand AddEmployeeCommand => _AddEmployeeCommand
        //    ??= new DelegateCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
        //private bool CanAddEmployeeCommandExecute() => true;
        //private async void OnAddEmployeeCommandExecuted()
        //{
        //    await employeesService.AddEmployeeAsync(EmployeeInForm);
        //}

        //#endregion

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
            try
            {
                switch (EmployeeInForm.Id)
                {
                    case 0:
                        await employeesService.AddAsync(EmployeeInForm);
                        Collection.Add(EmployeeInForm);
                        StatusMessage.Message = "Новый сотрудник добавлен";
                        break;
                    default:
                        await employeesService.EditAsync(EmployeeInForm);
                        StatusMessage.Message = "Сотрудник изменён";
                        break;
                }
                OnGetEmployeesCommandExecuted();
                ResetFormCommand.Execute(null);
            }
            catch (ArgumentException)
            {
                ErrorMessage.Message = "Не все поля заполнены";
            }
        }

        #endregion
    }
}
