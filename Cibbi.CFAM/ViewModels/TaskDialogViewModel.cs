using System.Reactive.Linq;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public struct TaskDialogButtonData
{
    public string Text { get; set; }
    public object Result { get; set; }
}

public partial class TaskDialogViewModel : ViewModelBase
{
    [Notify] private string _header;
    [Notify] private string _subHeader;
    [Notify] private string _content;
    [Notify] private double _progress;
    private Task<object> _showTask;
    private Action<double> _onProgressChanged;
    private Action _hideAction;

    internal TaskDialogViewModel(Task<object> showTask, Action<double> onProgressChanged, Action hideAction)
    {
        _showTask = showTask;
        _onProgressChanged = onProgressChanged;
        _hideAction = hideAction;
    }

    public void OnProgressChanged()
    {
        _onProgressChanged.Invoke(_progress);
    }

    public void Close()
    {
        Observable.Start(() => _hideAction.Invoke(), RxApp.MainThreadScheduler);
    }
}