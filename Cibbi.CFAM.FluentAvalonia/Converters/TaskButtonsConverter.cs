using System.Globalization;
using Avalonia.Data.Converters;
using Cibbi.CFAM.FluentAvalonia.ViewModels;
using Cibbi.CFAM.ViewModels;
using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.FluentAvalonia.Converters;

public class TaskButtonsConverter : IValueConverter
{
    // ReSharper disable once UnusedMember.Global
    public static readonly TaskButtonsConverter Instance = new();
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<TaskDialogButtonData> data)
        {
            return data.Select(x => new TaskDialogButton(x.Text, x.Result)).ToList();
        }

        return Enumerable.Empty<TaskDialogButton>().ToList();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {

        return null;
    }
}