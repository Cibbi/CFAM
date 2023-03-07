using System.Globalization;
using Avalonia.Data.Converters;
using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.Converters;

public class PaneStatePositionMultiConverter : IMultiValueConverter
{
    // ReSharper disable once UnusedMember.Global
    public static readonly PaneStatePositionMultiConverter Instance = new();
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2 || values[0] is not PanePosition position || values[1] is not PaneState state)
            return NavigationViewPaneDisplayMode.LeftCompact;

        if (position == PanePosition.Top)
        {
            return NavigationViewPaneDisplayMode.Top;
        }
        else
        {
            return state switch
            {
                PaneState.Auto => NavigationViewPaneDisplayMode.Auto,
                PaneState.Full => NavigationViewPaneDisplayMode.Left,
                PaneState.Compact => NavigationViewPaneDisplayMode.LeftCompact,
                PaneState.Minimal => NavigationViewPaneDisplayMode.LeftMinimal,
                _ => NavigationViewPaneDisplayMode.Auto
            };
        }
        
    }
}