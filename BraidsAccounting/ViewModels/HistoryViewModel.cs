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
            Title = "История операций";
        }

        #region Command GetHistory - Команда получить историю

        private ICommand? _GetHistoryCommand;
        /// <summary>Команда - получить историю</summary>
        public ICommand GetHistoryCommand => _GetHistoryCommand
            ??= new DelegateCommand<RecordsNumber?>(OnGetHistoryCommandExecuted, CanGetHistoryCommandExecute);
        private bool CanGetHistoryCommandExecute(RecordsNumber? mode) => true;
        private async void OnGetHistoryCommandExecuted(RecordsNumber? mode)
        {
            if (!mode.HasValue) return;
            switch (mode.Value)
            {
                case RecordsNumber.Last50:
                    Collection = new(await historyService.GetRangeAsync(50).ConfigureAwait(false));
                    break;
                case RecordsNumber.All:
                    Collection = new(await historyService.GetAllAsync().ConfigureAwait(false));
                    break;
            }
        }

        #endregion       

    }

    public enum RecordsNumber
    {
        All,
        Last50
    }
}
