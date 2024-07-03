using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using JetBrains.Annotations;

namespace CFAM.Behaviors;

[PublicAPI]
public class LostFocusBehavior : Trigger<TextBox>
{
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is null) return;
        AssociatedObject.LostFocus += LostFocus;
    }

    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is null) return;
        AssociatedObject.LostFocus -= LostFocus;
    }
    
    private void LostFocus(object? sender, RoutedEventArgs e)
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}