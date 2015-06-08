using System;
using System.ComponentModel;
using System.Windows.Data;

namespace RecEpee.Utilities
{
    static class ExtensionMethods
    {
        public static void SetSorting(this ICollectionView collectionView, string sortedProperty)
        {
            collectionView.SortDescriptions.Add(new SortDescription(sortedProperty, ListSortDirection.Ascending));

            var cvls = collectionView as ICollectionViewLiveShaping;

            if (cvls != null)
            {
                cvls.IsLiveSorting = true;
                cvls.LiveSortingProperties.Add(sortedProperty);
            }
        }

        public static void SetGrouping(this ICollectionView collectionView, string groupProperty)
        {
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription(groupProperty));

            var cvls = collectionView as ICollectionViewLiveShaping;

            if (cvls != null)
            {
                cvls.IsLiveGrouping = true;
                cvls.LiveGroupingProperties.Add(groupProperty);
            }
        }

        public static void SetFiltering(this ICollectionView collectionView, string filterProperty, Predicate<object> filter)
        {
            collectionView.Filter = filter;

            var cvls = collectionView as ICollectionViewLiveShaping;

            if (cvls != null)
            {
                cvls.IsLiveFiltering = true;
                cvls.LiveFilteringProperties.Add(filterProperty);
            }
        }
    }
}
