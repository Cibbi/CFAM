using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Windows;

public partial class MainWindow : Window, IActivatableView
{
    public MainWindow(bool enableWindowDebug = false)
    {
        InitializeComponent();
        
        if (enableWindowDebug)
        {
            this.AttachDevTools();
        }
        this.WhenActivated(disposables =>
        {
            if(DataContext is not RoutedWindowBaseViewModel viewModel) return;
            
            ViewHost.Children.Clear();
            
            if (viewModel.ViewLocator.ResolveView(viewModel) is not Control control) return;
            control.DataContext = viewModel;
            ViewHost.Children.Add(control);
        });
    }
}