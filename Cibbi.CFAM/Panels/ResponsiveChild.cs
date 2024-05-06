using Avalonia;
using Avalonia.Controls;

namespace Cibbi.CFAM.Panels;

public partial class ResponsiveChild : Panel
{
    public static readonly AttachedProperty<double> MinimumSizeProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveChild, Control, double>(
            "MinimumSize",
            defaultValue: 0,
            validate: v => v >= 0);

    protected override Size ArrangeOverride(Size finalSize)
    {
        bool yetToFind = true;
        foreach (var t in Children)
        {
            //get attached MinimumSizeProperty
            var minimumSize = t.GetValue(MinimumSizeProperty);

            if (finalSize.Width < minimumSize && yetToFind)
            {
                t.IsVisible = true;
                yetToFind = false;
            }
            else
            {
                t.IsVisible = false;
            }
        }
        
        return finalSize; // Returns the final Arranged size
    }
}