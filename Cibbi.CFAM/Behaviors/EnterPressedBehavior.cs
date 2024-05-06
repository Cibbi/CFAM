using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Cibbi.CFAM.Behaviors;

public class EnterPressedBehavior : Trigger<TextBox>
{
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is null) return;
        AssociatedObject.KeyDown += KeyDown;
    }
    
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is null) return;
        AssociatedObject.KeyDown -= KeyDown;
    }
    
    private void KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, e);
        }
    }
}