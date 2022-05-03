using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class SelectItemViewModel : ViewModelBase
{
    private readonly IViewService viewService;

    public SelectItemViewModel(IViewService viewService)
    {
        this.viewService = viewService;
    }

    /// <summary>
    /// Выбранный материал из каталога.
    /// </summary>
    public Item SelectedItem { get; set; } = null!;
   
    #region Command Select - Команда выбрать товар

    private ICommand? _SelectCommand;

    /// <summary>Команда - выбрать товар</summary>
    public ICommand SelectCommand => _SelectCommand
        ??= new DelegateCommand<Item>(OnSelectCommandExecuted, CanSelectCommandExecute);
    private bool CanSelectCommandExecute(Item item) => true;
    private void OnSelectCommandExecuted(Item item)
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
