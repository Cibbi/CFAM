using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        if(ViewModel is not null)
            ViewModel.GetRootViewModel += GetRoot;
        base.OnAttachedToLogicalTree(e);
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        if(ViewModel is not null)
            ViewModel.GetRootViewModel -= GetRoot;
        base.OnDetachedFromLogicalTree(e);
    }

    private WindowBaseViewModel? GetRoot()
    {
        return TopLevel.GetTopLevel(this)?
            .GetLogicalChildren()
            .OfType<StyledElement>()
            .FirstOrDefault(x => x.DataContext is WindowBaseViewModel)
            ?.DataContext as WindowBaseViewModel;
    }
}