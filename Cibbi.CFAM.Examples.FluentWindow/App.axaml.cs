using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.Services;
using FluentAvalonia.FluentIcons;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.FluentWindow
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            IconsProvider iconsProvider = new IconsProvider()
                .WithIconFactory("DocumentIcon", () => new FluentIcon{Icon = FluentIconSymbol.Document16Regular});

            AvaloniaLocator.CurrentMutable
                .BindToSelfSingleton<DialogProvider>()
                .Bind<IPagesProvider>().ToSingleton<PagesProvider>()
                .Bind<IIconsProvider>().ToConstant(iconsProvider)
                .Bind<IViewLocator>().ToSingleton<ViewLocator>();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = ViewLocator.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}