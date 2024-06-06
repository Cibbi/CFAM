using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace CFAM.Converters;

public class IconConverter : IValueConverter
{
    // ReSharper disable once UnusedMember.Global
    public static readonly IconConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text) throw new ArgumentOutOfRangeException(nameof(value));
        object? resource = null;

        var result = Application.Current != null && Application.Current.TryGetResource(text, Application.Current.ActualThemeVariant, out resource);
        if(result && resource is not null) return resource;

        throw new ArgumentOutOfRangeException(nameof(value));
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}