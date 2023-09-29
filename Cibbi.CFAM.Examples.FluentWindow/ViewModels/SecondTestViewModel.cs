using Cibbi.CFAM.Attributes;
using Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;
using Cibbi.CFAM.ViewModels;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.FluentWindow.ViewModels;

[Page("", UrlPath, "DocumentIcon")]
public partial class SecondTestViewModel : RoutableViewModel
{
    public const string UrlPath = "SecondPageTest";
    public override string UrlPathSegment => UrlPath;
    
    [Notify] private double _closedPaneWidth;
    [Notify] private double _openPaneWidth;
    
    public SecondTestViewModel(IScreen screen) : base(screen)
    {
    }

    protected override void OnRootViewModelSet()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
        {
            ClosedPaneWidth = fluent.ClosedPaneWidth;
            OpenPaneWidth = fluent.OpenPaneWidth;
        }
    }

    private void OnClosedPaneWidthChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.ClosedPaneWidth = ClosedPaneWidth;
    }
    
    private void OnOpenPaneWidthChanged()
    {
        if (RootViewModel is MainFluentWindowViewModel fluent)
            fluent.OpenPaneWidth = OpenPaneWidth;
    }
    
}