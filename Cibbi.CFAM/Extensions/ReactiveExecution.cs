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
    
    [UsedImplicitly]
    public static IObservable<Unit>ExecuteOnUIThreadAsync(Action action)
    {
        return ExecuteOnThreadAsync(action, RxApp.MainThreadScheduler);
    }
    
    [UsedImplicitly]
    public static IObservable<Unit> ExecuteOnTaskThreadAsync(Action action)
    {
        return ExecuteOnThreadAsync(action, RxApp.TaskpoolScheduler);
    }
}