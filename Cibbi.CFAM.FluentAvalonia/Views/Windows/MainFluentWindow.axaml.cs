using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Styling;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.FluentAvalonia.Services;
using Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;
using Cibbi.CFAM.ViewModels;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.FluentAvalonia.Views.Windows
{
    public partial class MainFluentWindow : ReactiveAppWindow<MainFluentWindowViewModel>
    {
        private IIconsProvider _provider = Locator.Current.GetRequiredService<IIconsProvider>();
        public MainFluentWindow()
        {
            this.WhenActivated(disposable =>
            {
                if (ViewModel is null) return;
                ViewModel.ObservableForProperty<MainFluentWindowViewModel, string>(x => x.MainListing)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => RefreshNavigationItems())
                    .DisposeWith(disposable);
                ViewModel.ObservableForProperty<MainFluentWindowViewModel, string>(x => x.OptionsListing)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ => RefreshNavigationFooterItems())
                    .DisposeWith(disposable);
                
                ViewModel?.Router.NavigationChanged.Subscribe(_ => OnNavigationChanged()).DisposeWith(disposable);
                ViewModel?.WhenAnyValue<MainFluentWindowViewModel, bool>(x => x.IsPaneToggleVisible)
                    .Subscribe(_ => OnOptionPaneToggleChanged()).DisposeWith(disposable);
            });
            InitializeComponent();
            RoutedViewHost.ViewLocator = Locator.Current.GetRequiredService<IViewLocator>();
            TitleBar.ExtendsContentIntoTitleBar = true;
            TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
            
            /*var thm = Locator.Current.GetRequiredService<FluentAvaloniaTheme>();
            thm.CustomAccentColor = Color.FromRgb(58,55,191);
            thm.ForceWin32WindowToTheme(this);*/
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            
            if (TitleBar != null)
            {
                TitleBar.ExtendsContentIntoTitleBar = true;
            }

            RefreshNavigationItems();
            RefreshNavigationFooterItems();

            NavMenu.ItemInvoked += OnNavMenuItemInvoked;
            NavMenu.BackRequested += OnNavMenuBackRequested;
        }

        private void RefreshNavigationItems()
        {
            NavMenu.MenuItemsSource = null;
            var navigationItems = new List<NavigationViewItem>();
            foreach (var page in ViewModel?.GetPages(ViewModel.MainListing) ?? Enumerable.Empty<Page>())
            {
                navigationItems.Add(new NavigationViewItem
                {
                    Content = page.Name,
                    Tag = page.PageType,
                    IconSource = _provider.GetIconFromName(page.IconName)
                });
            }

            NavMenu.MenuItemsSource = navigationItems;
            
            if (navigationItems.Count > 0 && navigationItems[0].Tag is Type typ)
                ViewModel?.NavigateTo(typ, true);
        }
        
        private void RefreshNavigationFooterItems()
        {
            NavMenu.FooterMenuItemsSource = null;
            if (string.IsNullOrEmpty(ViewModel?.OptionsListing)) return;
            var optionsItems = new List<NavigationViewItem>();
            foreach (var page in ViewModel?.GetPages(ViewModel.OptionsListing) ?? Enumerable.Empty<Page>())
            {
                optionsItems.Add(new NavigationViewItem
                {
                    Content = page.Name,
                    Tag = page.PageType,
                    IconSource =  _provider.GetIconFromName(page.IconName)
                });
            }

            NavMenu.FooterMenuItemsSource = optionsItems;
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
                await ani.RunAsync(TitleBarHost);
                NavMenu.IsBackButtonVisible = true;
            }
            else
            {
                NavMenu.IsBackButtonVisible = false;
                var ani = GetContentAnimation(openThickness, closedThickness);
                await ani.RunAsync(TitleBarHost);
            }
        }
        
        private async void AnimateContentForOptionPane(bool show)
        {
            var closedThickness = new Thickness(0, 0, 0, 0);
            var openThickness = new Thickness(ViewModel!.ClosedPaneWidth - 8, 0, 0, 0);
            if (show)
            {
                var ani = GetContentAnimation(closedThickness, openThickness);
                await ani.RunAsync(TitleBarHost);
            }
            else
            {
                var ani = GetContentAnimation(openThickness, closedThickness);
                await ani.RunAsync(TitleBarHost);
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
    }
}