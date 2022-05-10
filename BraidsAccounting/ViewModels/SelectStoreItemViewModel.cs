using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
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
                Notifier.AddError(Messages.ItemNotSelected);
                return;
            }
            viewService.AddParameter(ParameterNames.SelectedItem, SelectedItem);
            viewService.GoBack();
        }

        #endregion

    }
}
