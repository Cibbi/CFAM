using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Windows;

public partial class MainWindow : ReactiveCoreWindow<RoutedWindowBaseViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
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