using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class HistoryViewModel : ViewModelBase<History>
    {
        private readonly IHistoryService historyService;

        public HistoryViewModel(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        #region Command LoadData - Команда получить историю операций с сущностями 

        private ICommand? _LoadDataCommand;
        /// <summary>Команда - </summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new DelegateCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
        private bool CanLoadDataCommandExecute() => true;
        private async void OnLoadDataCommandExecuted() =>
            Collection = new(await historyService.GetAllAsync());

        #endregion
    }
}
