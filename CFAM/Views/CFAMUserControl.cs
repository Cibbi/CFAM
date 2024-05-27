using Avalonia.ReactiveUI;
using CFAM.ViewModels;

namespace CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
    }
}