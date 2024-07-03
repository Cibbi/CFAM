using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using JetBrains.Annotations;
using ReactiveUI;

namespace CFAM.Extensions;

public static class ListExtensions
{
    public static IObservable<Unit> AddRangeOnThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.AddRange(toAdd);
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> AddRangeOnUIThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd)
    {
        return enumerable.AddRangeOnThreadAsync(toAdd, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> AddRangeOnTaskThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toAdd)
    {
        return enumerable.AddRangeOnThreadAsync(toAdd, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> RemoveRangeOnThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toRemove, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Remove(toRemove);
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> RemoveRangeOnUIThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toRemove)
    {
        return enumerable.RemoveRangeOnThreadAsync(toRemove, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> RemoveRangeOnTaskThreadAsync<T>(this IList<T> enumerable, IEnumerable<T> toRemove)
    {
        return enumerable.RemoveRangeOnThreadAsync(toRemove, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> AddOnThreadAsync<T>(this IList<T> enumerable, T toAdd, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Add(toAdd);
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> AddOnUIThreadAsync<T>(this IList<T> enumerable, T toAdd)
    {
        return enumerable.AddOnThreadAsync(toAdd, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> AddOnTaskThreadAsync<T>(this IList<T> enumerable, T toAdd)
    {
        return enumerable.AddOnThreadAsync(toAdd, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> InsertOnThreadAsync<T>(this IList<T> enumerable, int index, T toAdd, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Insert(index, toAdd);
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> InsertOnUIThreadAsync<T>(this IList<T> enumerable, int index, T toAdd)
    {
        return enumerable.InsertOnThreadAsync(index, toAdd, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> InsertOnTaskThreadAsync<T>(this IList<T> enumerable, int index, T toAdd)
    {
        return enumerable.InsertOnThreadAsync(index, toAdd, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> RemoveOnThreadAsync<T>(this IList<T> enumerable, T toRemove, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Remove(toRemove);
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> RemoveOnUIThreadAsync<T>(this IList<T> enumerable, T toRemove)
    {
        return enumerable.RemoveOnThreadAsync(toRemove, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> RemoveOnTaskThreadAsync<T>(this IList<T> enumerable, T toRemove)
    {
        return enumerable.RemoveOnThreadAsync(toRemove, RxApp.TaskpoolScheduler);
    }
    
    public static IObservable<Unit> ClearOnThreadAsync<T>(this IList<T> enumerable, IScheduler scheduler)
    {
        return Observable.Start(() => {
            enumerable.Clear();
        }, scheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> ClearOnUIThreadAsync<T>(this IList<T> enumerable)
    {
        return enumerable.ClearOnThreadAsync(RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> ClearOnTaskThreadAsync<T>(this IList<T> enumerable)
    {
        return enumerable.ClearOnThreadAsync(RxApp.TaskpoolScheduler);
    }
}