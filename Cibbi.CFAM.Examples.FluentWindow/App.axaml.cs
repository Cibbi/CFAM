using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.Examples.FluentWindow.ViewModels;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.FluentAvalonia.Services;
using Cibbi.CFAM.Services;
using Cibbi.CFAM.ViewModels.Mains;
using Cibbi.CFAM.Views.Windows;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            IconsProvider iconsProvider = new IconsProvider()
                .WithIconFactory("DocumentIcon", () => new SymbolIconSource { Symbol = Symbol.Document });

            Locator.CurrentMutable.RegisterSingleton<DialogProvider>()
                .RegisterAnd<IPagesProvider, PagesProvider>()
                .RegisterConstantAnd<IIconsProvider>(iconsProvider)
                .RegisterSingleton<IViewLocator, ViewLocator>();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var context = new HeaderedMainViewModel("FluentAvalonia Example", ViewLocator.Current)
                {
                    WindowName = "Test",
                };
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = context
                };
                context.Router.Navigate.Execute(new TestViewModel(context));
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}