using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.Services;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.Examples.SimpleWindow
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            
            Locator.CurrentMutable
                .RegisterSingletonAnd<DialogProvider>()
                .RegisterSingletonAnd<IPagesProvider, PagesProvider>()
                .RegisterSingletonAnd<IViewLocator, ViewLocator>();
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