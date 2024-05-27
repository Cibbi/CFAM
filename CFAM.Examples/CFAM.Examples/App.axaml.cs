using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CFAM.Examples.ViewModels;
using CFAM.Examples.Views;
using CFAM.Extensions;
using CFAM.Services;
using CFAM.ViewModels.Mains;
using CFAM.Views.Mains;
using CFAM.Views.Windows;
using Splat;

namespace CFAM.Examples;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var viewLocator = new BaseViewLocator();
        var context = new HeaderedMainViewModel("CFAM Example", viewLocator);

        Locator.CurrentMutable.RegisterConstant<IViewLocator>(viewLocator);
        
#if DEBUG
        bool debug = true;
#else
        bool debug = false;
#endif
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow(debug)
                {
                    DataContext = context
                };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new HeaderedMainView
                {
                    DataContext = context
                };
                break;
        }
        
        context.Router.Navigate.Execute(new MainViewModel(context));

        base.OnFrameworkInitializationCompleted();
    }
}