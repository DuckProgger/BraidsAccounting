using BraidsAccounting.Modules;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class StoreItemWindowViewModel
    {
        private readonly IRegionManager regionManager;

        public StoreItemWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #region Command LoadAddItemViewCommand - Команда 

        private ICommand? _LoadAddItemViewCommand;
        /// <summary>Команда - </summary>
        public ICommand LoadAddItemViewCommand => _LoadAddItemViewCommand
            ??= new DelegateCommand(OnLoadAddItemViewCommandCommandExecuted, CanLoadAddItemViewCommandCommandExecute);
        private bool CanLoadAddItemViewCommandCommandExecute() => true;
        private async void OnLoadAddItemViewCommandCommandExecuted()
        {
            regionManager.RequestNavigate(StoreItemModule.RegionName, "EditStoreItemView");

        }

        #endregion

        #region Command NavigateToOtherView - Команда переключиться на другое представление

        private ICommand? _NavigateToOtherViewCommand;
        /// <summary>Команда - переключиться на другое представление</summary>
        public ICommand NavigateToOtherViewCommand => _NavigateToOtherViewCommand
            ??= new DelegateCommand<string>(OnNavigateToOtherViewCommandExecuted, CanNavigateToOtherViewCommandExecute);
        private bool CanNavigateToOtherViewCommandExecute(string viewName) => true;
        private async void OnNavigateToOtherViewCommandExecuted(string viewName)
        {
            regionManager.RequestNavigate(StoreItemModule.RegionName, viewName);

        }

        #endregion
    }
}
