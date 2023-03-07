using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace Cibbi.CFAM.Extensions;

public static class ListExtensions
{
    public static IObservable<Unit> AddRangeOnThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.AddRange(toAdd);
        }, scheduler);
    }
    
    public static IObservable<Unit> AddRangeOnUIThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd)
    {
        return enumerable.AddRangeOnThreadAsync(toAdd, RxApp.MainThreadScheduler);
    }
    
    public static IObservable<Unit> AddRangeOnTaskThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd)
    {
        return enumerable.AddRangeOnThreadAsync(toAdd, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> AddOnThreadAsync<T>(this IList<T> enumerable, T toAdd, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Add(toAdd);
        }, scheduler);
    }
    
    public static IObservable<Unit> AddOnUIThreadAsync<T>(this IList<T> enumerable, T toAdd)
    {
        return enumerable.AddOnThreadAsync(toAdd, RxApp.MainThreadScheduler);
    }
    
    public static IObservable<Unit> AddOnTaskThreadAsync<T>(this IList<T> enumerable, T toAdd)
    {
        return enumerable.AddOnThreadAsync(toAdd, RxApp.TaskpoolScheduler);
    }
}