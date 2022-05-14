using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class PopupViewModel : ViewModelBase
{
    private readonly IViewService viewService;

    public PopupViewModel(IViewService viewService)
    {
        this.viewService = viewService;
    }

    #region Command GoBack - Команда вернуться на предыдущий экран

    private ICommand? _GoBackCommand;
    /// <summary>Команда - вернуться на предыдущий экран</summary>
    public ICommand GoBackCommand => _GoBackCommand
        ??= new DelegateCommand(OnGoBackCommandExecuted, CanGoBackCommandExecute);
    private bool CanGoBackCommandExecute() => true;
    private void OnGoBackCommandExecuted()
    {
        viewService.GoBack();
    }

    #endregion
}

