using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.Services;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.SimpleWindow
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            
            AvaloniaLocator.CurrentMutable
                .BindToSelfSingleton<DialogProvider>()
                .Bind<IPagesProvider>().ToSingleton<PagesProvider>()
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