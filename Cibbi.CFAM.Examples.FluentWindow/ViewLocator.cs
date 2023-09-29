using Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;
using Cibbi.CFAM.FluentAvalonia.Views.Windows;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public class ViewLocator : BaseViewLocator
    {
        public static MainFluentWindowViewModel MainViewModel { get; } = new() {WindowName = "Fluent Window Test" };
        public static MainFluentWindow MainWindow { get; } = new() {DataContext = MainViewModel};
        public static ViewLocator Current { get; } = new();
    }
}