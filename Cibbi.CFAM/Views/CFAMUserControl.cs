using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.LogicalTree;
using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
        this.WhenActivated(disposable =>
        {
            if(ViewModel is null) return;
            if(ViewModel.GetRootViewModel is not null) return;

            ViewModel.GetRootViewModel = ReactiveCommand.Create(GetRoot);

            ViewModel.GetRootViewModel.DisposeWith(disposable);
        });
    }
    
    private WindowBaseViewModel? GetRoot()
    {
        return TopLevel.GetTopLevel(this)?
            .GetLogicalChildren()
            .OfType<UserControl>()
            .FirstOrDefault(x => x.DataContext is WindowBaseViewModel)
            ?.DataContext as WindowBaseViewModel;
    }
}