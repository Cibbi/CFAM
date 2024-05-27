using System.Globalization;
using Avalonia.Data.Converters;

namespace CFAM.Converters;

public class WidthToScaledWidthConverter : IValueConverter
{
    public static readonly WidthToScaledWidthConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double width && parameter is double scale)
        {
            return width / scale;
        }
        return 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double width && parameter is double scale)
        {
            return width * scale;
        }
        return 0;
    }
}