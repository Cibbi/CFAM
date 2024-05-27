using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using JetBrains.Annotations;

namespace CFAM.Behaviors.Actions;

[PublicAPI]
public class ButtonTriggerAction : AvaloniaObject, IAction
{
    public static readonly StyledProperty<Button> ButtonProperty =
        AvaloniaProperty.Register<ButtonTriggerAction, Button>(nameof(Button));

    public Button Button
    {
        get => GetValue(ButtonProperty);
        set => SetValue(ButtonProperty, value);
    }
    
    public object? Execute(object? sender, object? parameter)
    {
        Button.Command?.Execute(Button.CommandParameter);
        return null;
    }
}