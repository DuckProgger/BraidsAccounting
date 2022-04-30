using Prism.Regions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BraidsAccounting.Infrastructure
{
    internal class ViewModelBase : INotifyPropertyChanged, INavigationAware
    {
        public NewNotifier Notifier { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public virtual void OnNavigatedTo(NavigationContext navigationContext) { }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
    }

    internal class ViewModelBase<T> : ViewModelBase
    {
        public ObservableCollection<T> Collection { get; set; } = new();
    }
}
