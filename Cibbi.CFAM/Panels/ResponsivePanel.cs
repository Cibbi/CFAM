using Avalonia;
using Avalonia.Controls;

namespace Cibbi.CFAM.Panels;

public class ResponsivePanel : Panel
{
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<StackPanel, double>(nameof(Spacing), 0);
    
    public static readonly StyledProperty<double> SeparateEveryWidthProperty =
        AvaloniaProperty.Register<StackPanel, double>(nameof(SeparateEveryWidth), 800);
    
    public static readonly StyledProperty<int> MaxColumnsProperty =
        AvaloniaProperty.Register<StackPanel, int>(nameof(MaxColumns), 0);
    
    public static readonly StyledProperty<bool> HorizontalContentStretchProperty =
        AvaloniaProperty.Register<StackPanel, bool>(nameof(HorizontalContentStretch), false);
    
    public static readonly StyledProperty<bool> VerticalContentStretchProperty =
        AvaloniaProperty.Register<StackPanel, bool>(nameof(VerticalContentStretch), false);
    
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }
    
    public double SeparateEveryWidth
    {
        get => GetValue(SeparateEveryWidthProperty);
        set => SetValue(SeparateEveryWidthProperty, value);
    }
    
    public int MaxColumns
    {
        get => GetValue(MaxColumnsProperty);
        set => SetValue(MaxColumnsProperty, value);
    }
    
    public bool HorizontalContentStretch
    {
        get => GetValue(HorizontalContentStretchProperty);
        set => SetValue(HorizontalContentStretchProperty, value);
    }
    
    public bool VerticalContentStretch
    {
        get => GetValue(VerticalContentStretchProperty);
        set => SetValue(VerticalContentStretchProperty, value);
    }
    
    private int _effectiveColumns;
    protected override Size MeasureOverride(Size availableSize)
    {
        _effectiveColumns = (int) Math.Ceiling(availableSize.Width / Math.Max(SeparateEveryWidth, 1));
        
        if(_effectiveColumns == 0)
            _effectiveColumns = 1;
        if(_effectiveColumns > MaxColumns && MaxColumns != 0)
            _effectiveColumns = MaxColumns;
        
        var maxHeight = 0.0;
        for(int i = 0; i < Children.Count; i+= _effectiveColumns)
        {
            //measure height of each column and select the highest one
            var columnHeight = 0.0;
            for (int j = 0; j < _effectiveColumns; j++)
            {
                if (i + j >= Children.Count) continue;
                Children[i + j].Measure(availableSize);
                columnHeight = Math.Max(columnHeight, Children[i + j].DesiredSize.Height);
                columnHeight += Spacing;
            }
            maxHeight += columnHeight;
        }
        
        var panelDesiredSize = new Size(availableSize.Width, maxHeight);

        return panelDesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0.0;
        double y = 0.0;
        double columnHeight;
        for (int i = 0; i < Children.Count; i += _effectiveColumns)
        {
            //measure height of each column and select the highest one
            columnHeight = 0.0;
            for (int j = 0; j < _effectiveColumns; j++)
            {
                if (i + j >= Children.Count) continue;
                columnHeight = Math.Max(columnHeight, Children[i + j].DesiredSize.Height);
            }
            for (int j = 0; j < _effectiveColumns; j++)
            {
                if (i + j >= Children.Count) continue;
                
                double controlWidth = (finalSize.Width / _effectiveColumns) - Spacing;
                if (controlWidth < 0) controlWidth = 0;
                if (controlWidth > Children[i + j].DesiredSize.Width && !HorizontalContentStretch) controlWidth = Children[i + j].DesiredSize.Width;

                double controlHeight = columnHeight;
                if (controlHeight < 0) controlHeight = 0;
                if (controlHeight > Children[i + j].DesiredSize.Height && !VerticalContentStretch) controlHeight = Children[i + j].DesiredSize.Height;
                
                
                Children[i + j].Arrange(new Rect(x + Spacing / 2, y + Spacing / 2, controlWidth, controlHeight));
                x += finalSize.Width / _effectiveColumns;
            }
            x = 0;
            y += columnHeight + Spacing;
        }
        
        return finalSize; // Returns the final Arranged size
    }
}