using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.Windows;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Windows
{
    public partial class MainFluentWindow : ReactiveCoreWindow<MainFluentWindowViewModel>
    {
        public string MainListing { get; init; } = "";
        public string? OptionsListing { get; init; }
        public MainFluentWindow()
        {
            this.WhenActivated(_ => { });
            InitializeComponent();
            RoutedViewHost.ViewLocator = AvaloniaLocator.Current.GetRequiredService<IViewLocator>();
            var thm = AvaloniaLocator.Current.GetRequiredService<FluentAvaloniaTheme>();
            thm.CustomAccentColor = Color.FromRgb(58,55,191);
            thm.ForceWin32WindowToTheme(this);
            Width = 1280;
            Height = 750;
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            
            if (TitleBar != null)
            {
                TitleBar.ExtendViewIntoTitleBar = true;

               // TitleBar.LayoutMetricsChanged += OnApplicationTitleBarLayoutMetricsChanged;

                if (this.FindControl<Grid>("TitleBarHost") is Grid g)
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

            var navigationItems = new List<NavigationViewItem>();
            foreach (var page in ViewModel?.GetPages(MainListing) ?? Enumerable.Empty<Page>())
            {
                navigationItems.Add(new NavigationViewItem
                {
                    Content = page.Name,
                    Tag = page.PageType,
                    Icon = new IconSourceElement { IconSource = (IconSource)this.FindResource(page.IconName)! }
                });
            }
            
            NavMenu.MenuItems = navigationItems;
            
            if (!string.IsNullOrEmpty(OptionsListing))
            {
                var optionsItems = new List<NavigationViewItem>();
                foreach (var page in ViewModel?.GetPages(OptionsListing) ?? Enumerable.Empty<Page>())
                {
                    navigationItems.Add(new NavigationViewItem
                    {
                        Content = page.Name,
                        Tag = page.PageType,
                        Icon = new IconSourceElement {IconSource = (IconSource) this.FindResource(page.IconName)!}
                    });
                }

                NavMenu.FooterMenuItems = optionsItems;
            }

            NavMenu.ItemInvoked += OnNavMenuItemInvoked;
            NavMenu.BackRequested += OnNavMenuBackRequested;
            
            ViewModel?.Router.NavigationChanged.Subscribe(_ => OnNavigationChanged());
        }
        
        private void OnNavMenuItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            if (e.InvokedItemContainer is NavigationViewItem nvi && nvi.Tag is Type typ)
            {
                ViewModel?.NavigateTo(typ);
            }
        }
        
        private void OnNavMenuBackRequested(object? sender, NavigationViewBackRequestedEventArgs e)
        {
            ViewModel?.NavigateBack();
        }

        private void OnNavigationChanged()
        {
            if (ViewModel?.Router.NavigationStack.LastOrDefault() is RoutableViewModel currentPageType)
            {
                string rootPath = currentPageType.UrlPathSegment.Replace("\\", "/").Split("/").First();
                foreach (NavigationViewItem nvi in NavMenu.MenuItems)
                {
                    if (!nvi.Content?.Equals(rootPath) ?? true) continue;
                    NavMenu.SelectedItem = nvi;
                    break;
                }
            }
            
            int stackCount = ViewModel?.Router.NavigationStack.Count ?? 0;
            if(NavMenu.IsBackButtonVisible && stackCount <= 1)
                AnimateContentForBackButton(false);
            else if(!NavMenu.IsBackButtonVisible && stackCount > 1)
                AnimateContentForBackButton(true);
        }
        
        private async void AnimateContentForBackButton(bool show)
        {
            if (show)
            {
                var ani = new Animation
                {
                    Duration = TimeSpan.FromMilliseconds(250),
                    FillMode = FillMode.Forward,
                    Children =
                    {
                        new KeyFrame
                        {
                            Cue = new Cue(0d),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(0, 0, 0, 0))
                            }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(1d),
                            KeySpline = new KeySpline(0,0,0,1),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(36,0,0,0))
                            }
                        }
                    }
                };

                await ani.RunAsync((Animatable)TitleBarHost, null);

                NavMenu.IsBackButtonVisible = true;
            }
            else
            {
                NavMenu.IsBackButtonVisible = false;

                var ani = new Animation
                {
                    Duration = TimeSpan.FromMilliseconds(250),
                    FillMode = FillMode.Forward,
                    Children =
                    {
                        new KeyFrame
                        {
                            Cue = new Cue(0d),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(36, 0, 0, 0))
                            }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(1d),
                            KeySpline = new KeySpline(0,0,0,1),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(0,0,0,0))
                            }
                        }
                    }
                };

                await ani.RunAsync(TitleBarHost, null);
            }
        }
        
        private void OnApplicationTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (this.FindControl<Grid>("TitleBarHost") is Grid g)
            {
                g.Margin = new Thickness(0, 0, sender.SystemOverlayRightInset, 0);
            }
        }
        
        private void OnRequestedThemeChanged(FluentAvaloniaTheme sender, RequestedThemeChangedEventArgs args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: add Windows version to CoreWindow
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
}