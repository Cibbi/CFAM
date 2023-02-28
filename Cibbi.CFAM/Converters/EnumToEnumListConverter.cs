using System.Globalization;
using Avalonia.Data.Converters;

namespace Cibbi.CFAM.Converters;

public class EnumToEnumListConverter : IValueConverter
{
    // ReSharper disable once UnusedMember.Global
    public static EnumToEnumListConverter Instance { get; set; } = new();
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum e)
        {
            return Enum.GetValues(e.GetType());
        }

        return Array.Empty<object>();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum[] e && e.Length > 0)
        {
            return e[0];
        }

        return null;
    }
}