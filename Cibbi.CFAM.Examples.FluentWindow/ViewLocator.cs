using Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;
using Cibbi.CFAM.FluentAvalonia.Views.Windows;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public class ViewLocator : BaseViewLocator
    {
        public static ViewLocator Current { get; } = new();
    }
}