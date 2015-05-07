using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RecEpee.Framework
{
    class CollectionSyncher<T>
    {
        public static ObservableCollection<T> GetSynchedCollection(ICollection<T> list)
        {
            var observableCollection = new ObservableCollection<T>(list);
            observableCollection.CollectionChanged += collectionChanged;
            _map.Add(observableCollection, list);

            return observableCollection;
        }

        private static void collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var observableCollection = (ObservableCollection<T>)sender;

            if (observableCollection == null)
	        {
		        throw new ArgumentException("Not an instance of ObservableCollection", "sender");
	        }

            ICollection<T> list = getList(observableCollection);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    addItems(e.NewItems, list);
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    removeItems(e.OldItems, list);
                    break;

                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                default:
                    throw new NotImplementedException();
            }
        }

        private static void addItems(IEnumerable items, ICollection<T> list)
        {
            foreach (var newItem in items.Cast<T>())
            {
                list.Add(newItem);
            }
        }

        private static void removeItems(IEnumerable items, ICollection<T> list)
        {
            foreach (var newItem in items.Cast<T>())
            {
                list.Remove(newItem);
            }
        }

        private static ICollection<T> getList(ObservableCollection<T> observableCollection)
        {
            ICollection<T> list;
            bool valid = _map.TryGetValue(observableCollection, out list);

            if (valid == false)
            {
                throw new ArgumentException("Collection non registered", "observableCollection");
            }

            return list;
        }

        static ConditionalWeakTable<ObservableCollection<T>, ICollection<T>> _map = new ConditionalWeakTable<ObservableCollection<T>, ICollection<T>>();
    }

    class ColLectionSyncher
    {
        public static ObservableCollection<T> GetSynchedCollection<T>(ICollection<T> list)
        {
            return CollectionSyncher<T>.GetSynchedCollection(list);
        }
    }
}
