using System;
using System.Collections.Generic;
using Cibbi.CFAM.Examples.SimpleWindow.ViewModels;
using Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;
using Cibbi.CFAM.FluentAvalonia.Views.Windows;
using Cibbi.CFAM.Views;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.SimpleWindow
{
    public class ViewLocator : BaseViewLocator
    {
        public static MainSimpleWindowViewModel MainViewModel { get; } = new() { WindowName = "CFAM Simple Window", Content = new TestContentViewModel() };
        public static MainSimpleWindow MainWindow { get; } = new() { DataContext = MainViewModel };
        public static ViewLocator Current { get; } = new();

        
    }
}