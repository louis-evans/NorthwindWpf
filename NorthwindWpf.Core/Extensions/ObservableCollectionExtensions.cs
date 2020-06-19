using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthwindWpf.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> toAdd)
        {
            foreach(var item in toAdd)
            {
                collection.Add(item);
            }
        }
    }
}
