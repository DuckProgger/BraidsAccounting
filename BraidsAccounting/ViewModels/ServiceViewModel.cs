using BraidsAccounting.DAL.Entities;
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
        public Service Service { get; set; }
        public List<WastedItem> WastedItems { get; set; } = new();
        public WastedItem WastedItem { get; set; } = new();

        //public string Name { get; set; } = null!;
        //public decimal Profit { get; set; }

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
            serviceProvider.ProvideService(Service);
        }

        #endregion

        #region Command SelectStoreItem - Команда выбрать товар со склада

        private ICommand? _SelectStoreItemCommand;
        /// <summary>Команда - выбрать товар со склада</summary>
        public ICommand SelectStoreItemCommand => _SelectStoreItemCommand
            ??= new DelegateCommand(OnSelectStoreItemCommandExecuted, CanSelectStoreItemCommandExecute);
        private bool CanSelectStoreItemCommandExecute() => true;
        private async void OnSelectStoreItemCommandExecuted()
        {

        }

        #endregion
    }
}
