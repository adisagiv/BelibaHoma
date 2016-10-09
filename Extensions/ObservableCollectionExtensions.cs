using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Extensions
{
    public static class ObservableCollectionExtentions
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> shows)
        {

            var observableCollection = new ObservableCollection<TSource>(shows.ToList());

            return observableCollection;
        }
    }
}
