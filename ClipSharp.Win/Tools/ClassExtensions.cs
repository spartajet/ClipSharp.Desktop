using System.Collections.ObjectModel;

namespace ClipSharp.Win.Tools;

public static class ClassExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            collection.Add(item);
        }
    }
}