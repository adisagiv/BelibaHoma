using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.ObservableCollection
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
