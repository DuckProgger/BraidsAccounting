using BraidsAccounting.Modules;
using BraidsAccounting.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;

namespace BraidsAccounting.ViewModels
{
    internal class PopupViewModel : BindableBase
    {
        private readonly IViewService viewService;

        public PopupViewModel(IViewService viewService)
        {
            this.viewService = viewService;
        }
        public string Title { get; private set; }

    }
}

