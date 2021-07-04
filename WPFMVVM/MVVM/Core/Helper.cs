using System.ComponentModel;
using System.Windows.Data;

namespace NoiseCast.MVVM.Core
{
    public static class Helper
    {
        /// <summary>
        /// Sort a <see cref="ListCollectionView"/> dependen on a Property
        /// </summary>
        /// <param name="list"></param>
        /// <param name="property">The name of the property to sort</param>
        /// <param name="direction"><see cref="ListSortDirection.Ascending"/> or <see cref="ListSortDirection.Descending"/></param>
        public static void SortViewCollection(ListCollectionView list, string property, ListSortDirection direction)
        {
            list.SortDescriptions.Clear();
            list.SortDescriptions.Add(new SortDescription(property, direction));
        }
    }
}