using Avalonia;
using Avalonia.Xaml.Interactivity;
using CFAM.ViewModels;

namespace CFAM.Behaviors.WindowHandlers;

public abstract class WindowServiceHandler : Behavior<Visual>
{
    protected WindowServiceProvider? Provider;
    
    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        CheckDataContext();
        AssociatedObject!.DataContextChanged += AssociatedObjectOnDataContextChanged;
    }
    
    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        CheckDataContext();
        AssociatedObject!.DataContextChanged -= AssociatedObjectOnDataContextChanged;
    }

    private void AssociatedObjectOnDataContextChanged(object? sender, EventArgs e)
    {
        CheckDataContext();
    }

    private void CheckDataContext()
    {
        if (AssociatedObject!.DataContext is not IWindowServiceProvider provider) return;
        Provider = provider.WindowServices;
        OnDataContextChanged();
    }

    protected virtual void OnDataContextChanged(){}
}