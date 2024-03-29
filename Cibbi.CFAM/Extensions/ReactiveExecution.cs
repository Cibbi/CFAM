using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using ReactiveUI;

namespace Cibbi.CFAM.Extensions;

public static class ReactiveExecution
{
    public static IObservable<Unit> ExecuteOnThreadAsync(Action action, IScheduler scheduler)
    {
        return Observable.Start(action, scheduler);
    }
    
    public static IObservable<TResult> ExecuteOnThreadAsync<TResult>(Func<Task<TResult>> action, IScheduler scheduler)
    {
        return Observable.StartAsync(action, scheduler);
    }
    
    public static IObservable<Unit> ExecuteOnThreadAsync(Func<Task> action, IScheduler scheduler)
    {
        return Observable.StartAsync(action, scheduler);
    }
    
    [PublicAPI]
    public static IObservable<Unit> ExecuteOnUIThreadAsync(Action action)
    {
        return ExecuteOnThreadAsync(action, RxApp.MainThreadScheduler);
    }
    
    [PublicAPI]
    public static IObservable<TResult> ExecuteOnUIThreadAsync<TResult>(Func<Task<TResult>>  action)
    {
        return ExecuteOnThreadAsync(action, RxApp.MainThreadScheduler);
    }
    
    [PublicAPI]
    public static IObservable<Unit> ExecuteOnUIThreadAsync(Func<Task>  action)
    {
        return ExecuteOnThreadAsync(action, RxApp.MainThreadScheduler);
    }
    
    [PublicAPI]
    public static IObservable<Unit> ExecuteOnTaskThreadAsync(Action action)
    {
        return ExecuteOnThreadAsync(action, RxApp.TaskpoolScheduler);
    }
    
    [PublicAPI]
    public static IObservable<TResult> ExecuteOnTaskThreadAsync<TResult>(Func<Task<TResult>>  action)
    {
        return ExecuteOnThreadAsync(action, RxApp.TaskpoolScheduler);
    }
    
    [PublicAPI]
    public static IObservable<Unit> ExecuteOnTaskThreadAsync(Func<Task>  action)
    {
        return ExecuteOnThreadAsync(action, RxApp.TaskpoolScheduler);
    }
}