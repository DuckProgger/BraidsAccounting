using BraidsAccounting.Modules;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class StatisticsViewModel
    {
        private readonly IRegionManager regionManager;

        public StatisticsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #region Command NavigateToOtherView - Команда переключиться на другое представление

        private ICommand? _NavigateToOtherViewCommand;
        /// <summary>Команда - переключиться на другое представление</summary>
        public ICommand NavigateToOtherViewCommand => _NavigateToOtherViewCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherViewCommandExecuted, CanNavigateToOtherViewCommandExecute);
        private bool CanNavigateToOtherViewCommandExecute(string viewName) => true;
        private void OnNavigateToOtherViewCommandExecuted(string viewName) =>
            regionManager.RequestNavigate(RegionNames.Statistics, viewName);

        #endregion
    }
}
