using JetBrains.Annotations;

namespace CFAM;

[PublicAPI]
public class ThrottledTask
{
    private readonly Action _action;
    private readonly TimeSpan _delay;
    private readonly object _lockObject = new();
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;

    public ThrottledTask(Action action, TimeSpan delay)
    {
        _action = action;
        _delay = delay;
    }

    public void Run()
    {
        lock (_lockObject)
        {
            if (_isRunning)
            {
                _cancellationTokenSource?.Cancel();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
        }

        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(_delay, _cancellationTokenSource.Token);
                _action();
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                lock (_lockObject)
                {
                    _isRunning = false;
                }
            }
        }, _cancellationTokenSource.Token);
    }
}