using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
        SetRootViewModel();
    }

    private void SetRootViewModel()
    {
        if (ViewModel is null) return;
        switch (Application.Current?.ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var window1 = (desktop.Windows).FirstOrDefault(window2 => window2.IsActive);
                window1 ??= desktop.MainWindow;
                if (window1.DataContext is WindowBaseViewModel vm)
                    ViewModel.RootViewModel = vm;
                break;
            case ISingleViewApplicationLifetime view:
                if (view.MainView.DataContext is WindowBaseViewModel vm1)
                    ViewModel.RootViewModel = vm1;
                break;
        }
    }
}