using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace BraidsAccounting.Infrastructure
{
    internal abstract class FilterableBindableBase<T> : BindableBase
    {
        private ObservableCollection<T> collection = new();
        protected CollectionView collectionView = null!;

        /// <summary>
        /// Коллекция фильтруемых объектов.
        /// </summary>
        public ObservableCollection<T> Collection
        {
            get => collection;
            set
            {
                collection = value;
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(collection);
                collectionView.Filter = Filter;
            }
        }

        /// <summary>
        /// Предикат фильтрации списка.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract bool Filter(object obj);

    }
}
