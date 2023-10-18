using System.Reactive.Disposables;
using Avalonia.Controls;
using Cibbi.CFAM.ViewModels.Mains;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Mains;

public partial class HeaderedMainView : CFAMUserControl<HeaderedMainViewModel>
{
    public HeaderedMainView()
    {
        InitializeComponent();
        this.WhenActivated(disposable =>
        {
            if (ViewModel is null) return;
            ViewModel.Router.NavigationChanged.Subscribe(_ =>
            {
                var x = ViewModel.Router.GetCurrentViewModel();
                if(x is null) return;
                MainContent.Children.Clear();
                if (ViewModel.ViewLocator.ResolveView(x) is not Control control) return;
                control.DataContext = x;
                MainContent.Children.Add(control);
                
                if (ViewModel.Router.NavigationStack.Count > 1)
                    BackButton.Classes.Add("visible");
                else
                    BackButton.Classes.Remove("visible");

            }).DisposeWith(disposable);
        });
    }
}