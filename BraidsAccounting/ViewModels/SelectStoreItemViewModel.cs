using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class SelectStoreItemViewModel : ViewModelBase
    {
        private readonly IViewService viewService;

        public SelectStoreItemViewModel(IViewService viewService)
        {
            this.viewService = viewService;
        }

        /// <summary>
        /// Выбранный материал из каталога.
        /// </summary>
        public StoreItem SelectedItem { get; set; } = null!;

        #region Command Select - Команда выбрать товар

        private ICommand? _SelectCommand;

        /// <summary>Команда - выбрать товар</summary>
        public ICommand SelectCommand => _SelectCommand
            ??= new DelegateCommand<StoreItem>(OnSelectCommandExecuted, CanSelectCommandExecute);
        private bool CanSelectCommandExecute(StoreItem item) => true;
        private void OnSelectCommandExecuted(StoreItem item)
        {
            SelectedItem = item;
            if (SelectedItem is null)
            {
                Notifier.AddError(MessageContainer.ItemNotSelected);
                return;
            }
            viewService.AddParameter(ParameterNames.SelectedItem, SelectedItem);
            viewService.GoBack();
        }

        #endregion

        #region Command GoBack - Команда перейти на предыдущий экран

        private ICommand? _GoBackCommand;
        /// <summary>Команда - перейти на предыдущий экран</summary>
        public ICommand GoBackCommand => _GoBackCommand
            ??= new DelegateCommand(OnGoBackCommandExecuted, CanGoBackCommandExecute);
        private bool CanGoBackCommandExecute() => true;
        private void OnGoBackCommandExecuted() => viewService.GoBack();

        #endregion

    }
}
