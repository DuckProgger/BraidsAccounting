using BraidsAccounting.DAL.Entities;
using BraidsAccounting.Infrastructure;
using BraidsAccounting.Infrastructure.Constants;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels;

internal class SelectItemViewModel : ViewModelBase
{
    private readonly IViewService viewService;

    public SelectItemViewModel(IViewService viewService)
    {
        this.viewService = viewService;
        Title = "Выбор материала из каталога";
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
            Notifier.AddError(Resources.ItemNotSelected);
            return;
        }
        viewService.AddParameter(ParameterNames.SelectedItem, SelectedItem);
        viewService.GoBack();
    }

    #endregion

}
