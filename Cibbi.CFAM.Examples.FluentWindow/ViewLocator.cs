using System;
using System.Collections.Generic;
using Cibbi.CFAM.ViewModels.Windows;
using Cibbi.CFAM.Views;
using Cibbi.CFAM.Views.Windows;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public class ViewLocator : IViewLocator
    {
        public static MainFluentWindowViewModel MainViewModel { get; } = new() {WindowName = "Fluent Window Test"};
        public static MainFluentWindow MainWindow { get; } = new() {DataContext = MainViewModel};
        public static ViewLocator Current { get; } = new();
        
        private static readonly Dictionary<Type, Type?> _viewModelToViewMapping = new();

        public IViewFor ResolveView<T>(T viewModel, string? contract = null)
        {
            Type? vmType = viewModel?.GetType();
            if (vmType is null) throw new ArgumentNullException(nameof(viewModel));
            if(_viewModelToViewMapping.TryGetValue(vmType, out var vType)) return (IViewFor)Activator.CreateInstance(vType!)!;

            string? name = vmType.FullName!.Replace("ViewModel", "View");
            if(name is null) throw new ArgumentOutOfRangeException(nameof(viewModel));
            var type = Type.GetType(name);
            if (type is null || !type.IsAssignableTo(typeof(IViewFor)))
            {
                _viewModelToViewMapping.Add(vmType, typeof(AutoView));
                return (IViewFor)Activator.CreateInstance(typeof(AutoView))!;
            }
            _viewModelToViewMapping.Add(vmType, type);
            return (IViewFor)Activator.CreateInstance(type)!;
        }
    }
}