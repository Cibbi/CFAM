using Cibbi.CFAM.Examples.FluentWindow.ViewModels;
using Cibbi.CFAM.Views;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.FluentWindow.Views;

public partial class TestView : CFAMUserControl<TestViewModel>
{
    public TestView()
    {
        this.WhenActivated(disposable => { });
        InitializeComponent();
    }
}