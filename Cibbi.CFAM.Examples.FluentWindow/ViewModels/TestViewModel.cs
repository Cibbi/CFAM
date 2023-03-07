using Cibbi.CFAM.Attributes;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.Windows;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

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

    public TestViewModel(IScreen screen) : base(screen)
    {
        
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