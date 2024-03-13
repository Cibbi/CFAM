using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
    }
}