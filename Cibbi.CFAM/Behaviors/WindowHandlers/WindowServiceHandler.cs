using Avalonia;
using Avalonia.Xaml.Interactivity;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Behaviors.WindowHandlers;

public abstract class WindowServiceHandler : Behavior<Visual>
{
    protected WindowServiceProvider? Provider;
    
    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        AssociatedObject!.DataContextChanged += AssociatedObjectOnDataContextChanged;
    }
    
    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        AssociatedObject!.DataContextChanged -= AssociatedObjectOnDataContextChanged;
    }

    private void AssociatedObjectOnDataContextChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject!.DataContext is IWindowServiceProvider provider)
        {
            Provider = provider.WindowServices;
            OnDataContextChanged();
        }
    }

    protected virtual void OnDataContextChanged(){}
}