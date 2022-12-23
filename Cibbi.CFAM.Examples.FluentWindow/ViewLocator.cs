using System;
using Cibbi.CFAM.ViewModels.Windows;
using Cibbi.CFAM.Views.Windows;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public class ViewLocator : IViewLocator
    {
        public static MainFluentWindowViewModel MainViewModel { get; } = new() {WindowName = "Fluent Window Test"};
        public static MainFluentWindow MainWindow { get; } = new() {DataContext = MainViewModel};
        public static ViewLocator Current { get; } = new();

        public IViewFor ResolveView<T>(T viewModel, string? contract = null)
        {
            string? name = viewModel?.GetType().FullName!.Replace("ViewModel", "View");
            if (name is null) throw new ArgumentOutOfRangeException(nameof(viewModel));
            var type = Type.GetType(name);
            if (type is null) throw new ArgumentOutOfRangeException(nameof(viewModel));
            return (IViewFor) Activator.CreateInstance(type)!;
        }
    }
}