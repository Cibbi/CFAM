using JetBrains.Annotations;

namespace Cibbi.CFAM;

public class RepeatedTask : IDisposable
{
    [PublicAPI]
    public bool IsRunning { get; set; }
    
    private readonly Action _action;
    private readonly TimeSpan _interval;
    private readonly bool _runOnStart;
    private CancellationTokenSource? _cancellationTokenSource;

    private readonly object _lockObject = new();
    
    private Task _task = Task.CompletedTask;

    public RepeatedTask(Action action, TimeSpan interval, bool runOnStart = false)
    {
        _action = action;
        _interval = interval;
        _runOnStart = runOnStart;
    }

    public Task Task => _task;

    public void Stop()
    {
        lock (_lockObject)
        {
            if (!IsRunning)
                return;
            
            _cancellationTokenSource?.Cancel();
            IsRunning = false;
        }
    }

    public void Start()
    {
        lock (_lockObject)
        {
            if (IsRunning)
            {
                _cancellationTokenSource?.Cancel();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            IsRunning = true;
        }
        _task = Task.Run(async () =>
        {
            
            try
            {
                if (_runOnStart)
                    _action();
                
                while (!_cancellationTokenSource.IsCancellationRequested && IsRunning)
                {
                    await Task.Delay(_interval, _cancellationTokenSource.Token);
                    _action();
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                lock (_lockObject)
                {
                    IsRunning = false;
                }
            }
        }, _cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        Stop();
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _task.Dispose();
    }
}