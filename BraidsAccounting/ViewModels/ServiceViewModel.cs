using BraidsAccounting.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class ServiceViewModel
    {
        public List<ServiceFormItem> ServiceFormItems { get; set; }

        private readonly Services.Interfaces.IServiceProvider serviceProvider;

        public ServiceViewModel(Services.Interfaces.IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #region Command CreateService - Добавление сервиса

        private ICommand _CreateServiceCommand;
        /// <summary>Команда - Добавление сервиса</summary>
        public ICommand CreateServiceCommand => _CreateServiceCommand
            ??= new DelegateCommand(OnCreateServiceCommandExecuted, CanCreateServiceCommandExecute);
        private bool CanCreateServiceCommandExecute() => true;
        private async void OnCreateServiceCommandExecuted()
        {

        }

        #endregion

    }
}
