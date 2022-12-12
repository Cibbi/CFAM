using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Cibbi.CFAM.ViewModels.Windows;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Media;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Windows;

public partial class MainSimpleWindow : ReactiveCoreWindow<MainSimpleWindowViewModel>
{
    public MainSimpleWindow()
    {
        InitializeComponent();
        
        ContentHost.ViewLocator = AvaloniaLocator.Current.GetRequiredService<IViewLocator>();
        
        var thm = AvaloniaLocator.Current.GetRequiredService<FluentAvaloniaTheme>();
        thm.CustomAccentColor = Color.FromRgb(58,55,191);
        thm.ForceWin32WindowToTheme(this);
    }
    
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
            
        if (TitleBar != null)
        {
            TitleBar.ExtendViewIntoTitleBar = true;

            // TitleBar.LayoutMetricsChanged += OnApplicationTitleBarLayoutMetricsChanged;

            if (this.FindControl<Grid>("TitleBarHost") is { } g)
            {
                SetTitleBar(g);
                g.Margin = new Thickness(0, 0, TitleBar.SystemOverlayRightInset, 0);
            }
        }

        var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
        if(thm is not null)
            thm.RequestedThemeChanged += OnRequestedThemeChanged;

        // Enable Mica on Windows 11
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (IsWindows11 && thm is not null && thm.RequestedTheme != FluentAvaloniaTheme.HighContrastModeString)
            {
                TransparencyBackgroundFallback = Brushes.Transparent;
                TransparencyLevelHint = WindowTransparencyLevel.Mica;

                TryEnableMicaEffect(thm);
            }
        }

        thm?.ForceWin32WindowToTheme(this);
    }
    
    private void OnRequestedThemeChanged(FluentAvaloniaTheme sender, RequestedThemeChangedEventArgs args)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
        if (IsWindows11 && args.NewTheme != FluentAvaloniaTheme.HighContrastModeString)
        {
            TryEnableMicaEffect(sender);
        }
        else if (args.NewTheme == FluentAvaloniaTheme.HighContrastModeString)
        {
            // Clear the local value here, and let the normal styles take over for HighContrast theme
            SetValue(BackgroundProperty, AvaloniaProperty.UnsetValue);
        }
    }
    
    private void TryEnableMicaEffect(FluentAvaloniaTheme thm)
    {
        if (thm.RequestedTheme == FluentAvaloniaTheme.DarkModeString)
        {
            Color2 color = this.TryFindResource("SolidBackgroundFillColorBase", out var value) && value is Color v ? v : new Color2(32, 32, 32);

            color = color.LightenPercent(-0.8f);

            Background = new ImmutableSolidColorBrush(color, 0.78);
        }
        else if (thm.RequestedTheme == FluentAvaloniaTheme.LightModeString)
        {
            // Similar effect here
            Color2 color = this.TryFindResource("SolidBackgroundFillColorBase", out var value) && value is Color v ? v : new Color2(243, 243, 243);

            color = color.LightenPercent(0.5f);

            Background = new ImmutableSolidColorBrush(color, 0.9);
        }
    }
}