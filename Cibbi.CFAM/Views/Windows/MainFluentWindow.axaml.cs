using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Cibbi.CFAM.Services;
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
        private IIconsProvider _provider = AvaloniaLocator.Current.GetRequiredService<IIconsProvider>();
        public MainFluentWindow()
        {
            this.WhenActivated(disposable =>
            {
                if (ViewModel is null) return;
                ViewModel.ObservableForProperty(x => x.MainListing)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => RefreshNavigationItems())
                    .DisposeWith(disposable);
                ViewModel.ObservableForProperty(x => x.OptionsListing)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => RefreshNavigationFooterItems())
                    .DisposeWith(disposable);
                
                ViewModel?.Router.NavigationChanged.Subscribe(_ => OnNavigationChanged()).DisposeWith(disposable);
                ViewModel?.WhenAnyValue(x => x.IsPaneToggleVisible)
                    .Subscribe(_ => OnOptionPaneToggleChanged()).DisposeWith(disposable);
            });
            InitializeComponent();
            RoutedViewHost.ViewLocator = AvaloniaLocator.Current.GetRequiredService<IViewLocator>();
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

            RefreshNavigationItems();
            RefreshNavigationFooterItems();

            NavMenu.ItemInvoked += OnNavMenuItemInvoked;
            NavMenu.BackRequested += OnNavMenuBackRequested;
        }

        private void RefreshNavigationItems()
        {
            NavMenu.MenuItems = null;
            var navigationItems = new List<NavigationViewItem>();
            foreach (var page in ViewModel?.GetPages(ViewModel.MainListing) ?? Enumerable.Empty<Page>())
            {
                navigationItems.Add(new NavigationViewItem
                {
                    Content = page.Name,
                    Tag = page.PageType,
                    Icon = _provider.GetIconFromName(page.IconName)
                });
            }

            NavMenu.MenuItems = navigationItems;
            
            if (navigationItems.Count > 0 && navigationItems[0].Tag is Type typ)
                ViewModel?.NavigateTo(typ, true);
        }
        
        private void RefreshNavigationFooterItems()
        {
            NavMenu.FooterMenuItems = null;
            if (string.IsNullOrEmpty(ViewModel?.OptionsListing)) return;
            var optionsItems = new List<NavigationViewItem>();
            foreach (var page in ViewModel?.GetPages(ViewModel.OptionsListing) ?? Enumerable.Empty<Page>())
            {
                optionsItems.Add(new NavigationViewItem
                {
                    Content = page.Name,
                    Tag = page.PageType,
                    Icon =  _provider.GetIconFromName(page.IconName)
                });
            }

            NavMenu.FooterMenuItems = optionsItems;
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
            if (ViewModel?.IsPaneToggleVisible ?? false)
            {
                if(NavMenu.IsBackButtonVisible && stackCount <= 1)
                    NavMenu.IsBackButtonVisible = false;
                else if(!NavMenu.IsBackButtonVisible && stackCount > 1)
                    NavMenu.IsBackButtonVisible = true;
                return;
            }
            if(NavMenu.IsBackButtonVisible && stackCount <= 1)
                AnimateContentForBackButton(false);
            else if(!NavMenu.IsBackButtonVisible && stackCount > 1)
                AnimateContentForBackButton(true);
        }
        
        private void OnOptionPaneToggleChanged()
        {
            if((ViewModel?.IsPaneToggleVisible ?? false) && !NavMenu.IsBackButtonVisible)
                AnimateContentForOptionPane(true);
            if(!(ViewModel?.IsPaneToggleVisible ?? false) && !NavMenu.IsBackButtonVisible)
                AnimateContentForOptionPane(false);
        }
        
        private async void AnimateContentForBackButton(bool show)
        {
            var closedThickness = new Thickness(0, 0, 0, 0);
            var openThickness = new Thickness(ViewModel!.ClosedPaneWidth - 8, 0, 0, 0);
            if (show)
            {
                var ani = GetContentAnimation(closedThickness, openThickness);
                await ani.RunAsync(TitleBarHost, null);
                NavMenu.IsBackButtonVisible = true;
            }
            else
            {
                NavMenu.IsBackButtonVisible = false;
                var ani = GetContentAnimation(openThickness, closedThickness);
                await ani.RunAsync(TitleBarHost, null);
            }
        }
        
        private async void AnimateContentForOptionPane(bool show)
        {
            var closedThickness = new Thickness(0, 0, 0, 0);
            var openThickness = new Thickness(ViewModel!.ClosedPaneWidth - 8, 0, 0, 0);
            if (show)
            {
                var ani = GetContentAnimation(closedThickness, openThickness);
                await ani.RunAsync(TitleBarHost, null);
            }
            else
            {
                var ani = GetContentAnimation(openThickness, closedThickness);
                await ani.RunAsync(TitleBarHost, null);
            }
        }

        private Animation GetContentAnimation(Thickness startThickness, Thickness endThickness)
        {
            return new Animation
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
                            new Setter(MarginProperty, startThickness)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1d),
                        KeySpline = new KeySpline(0,0,0,1),
                        Setters =
                        {
                            new Setter(MarginProperty, endThickness)
                        }
                    }
                }
            };
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
}