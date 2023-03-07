using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Cibbi.CFAM.Converters;

public class WindowStateConverter : IValueConverter
{
    // ReSharper disable once UnusedMember.Global
    public static readonly WindowStateConverter Instance = new();
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ViewModels.WindowState state)
        {
            return state switch
            {
                ViewModels.WindowState.Normal => WindowState.Normal,
                ViewModels.WindowState.Minimized => WindowState.Minimized,
                ViewModels.WindowState.Maximized => WindowState.Maximized,
                //ViewModels.WindowState.FullScreen => WindowState.FullScreen,
                _ => WindowState.Normal
            };
        }
        return WindowState.Normal;
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is WindowState state)
        {
            return state switch
            {
                WindowState.Normal => ViewModels.WindowState.Normal,
                WindowState.Minimized => ViewModels.WindowState.Minimized,
                WindowState.Maximized => ViewModels.WindowState.Maximized,
                WindowState.FullScreen => ViewModels.WindowState.Maximized, //ViewModels.WindowState.FullScreen,
                _ => ViewModels.WindowState.Normal
            };
        }
        return ViewModels.WindowState.Normal;
    }
}