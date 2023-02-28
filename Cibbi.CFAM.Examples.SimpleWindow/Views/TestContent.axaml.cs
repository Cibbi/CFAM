using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.Examples.SimpleWindow.ViewModels;
using Cibbi.CFAM.Views;

namespace Cibbi.CFAM.Examples.SimpleWindow.Views;

public partial class TestContentView : CFAMUserControl<TestContentViewModel>
{
    public TestContentView()
    {
        InitializeComponent();
    }
}