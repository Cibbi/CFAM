using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Windows;

public partial class MainWindow : Window, IActivatableView
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            if(DataContext is not RoutedWindowBaseViewModel viewModel) return;
            ViewHost.ViewLocator = viewModel.ViewLocator;
            ViewHost.ViewModel = viewModel;
        });
    }
}