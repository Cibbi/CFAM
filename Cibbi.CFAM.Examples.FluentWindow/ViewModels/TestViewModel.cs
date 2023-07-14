using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Cibbi.CFAM.Attributes;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.Services;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.Windows;
using PropertyChanged.SourceGenerator;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.Examples.FluentWindow.ViewModels;

[Page("", UrlPath, "DocumentIcon")]
public partial class TestViewModel : RoutableViewModel
{
    public const string UrlPath = "Test";
    public override string UrlPathSegment => UrlPath;

    [Notify] private PanePosition _position;
    [Notify] private PaneState _state;
    [Notify] private WindowState _windowState;
    [Notify] private bool _isPaneToggleVisible;

    private DialogProvider _provider = Locator.Current.GetRequiredService<DialogProvider>();
    
    public ReactiveCommand<Unit, Unit> ShowDialogCommand { get; set; }

    public TestViewModel(IScreen screen) : base(screen)
    {
        ShowDialogCommand = ReactiveCommand.CreateFromTask(ShowDialog);
    }

    private async Task ShowDialog()
    {
        var taskDialog = _provider.GetTaskDialog();
        taskDialog.Header = "test";
        taskDialog.SubHeader = "test sub";
        taskDialog.Content = "Description";


        foreach (int index in Enumerable.Range(0, 100))
        {
            taskDialog.Progress = index;
            await Task.Delay(100);
        }
        taskDialog.Close();
    }

    protected override void OnRootViewModelSet()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
        {
            Position = fluent.PanePosition;
            State = fluent.PaneState;
            IsPaneToggleVisible = fluent.IsPaneToggleVisible;
        }
    }

    private void OnPositionChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.PanePosition = Position;
    }

    private void OnWindowStateChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.WindowState = WindowState;
    }
    
    private void OnStateChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.PaneState = State;
    }
    
    private void OnIsPaneToggleVisibleChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.IsPaneToggleVisible = IsPaneToggleVisible;
    }
}