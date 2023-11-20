using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Dialogs;

public partial class DialogsOverlayView : CFAMUserControl<DialogsOverlayViewModel>
{
    public DialogsOverlayView()
    {
        InitializeComponent();
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            if(ViewModel is null) return;
            
            ViewModel.WhenAnyValue(x => x.CurrentDialog)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    DialogContent.Children.Clear();
                    if (x is null) return;

                    if (CFAMSettings.ViewLocator.ResolveView(x) is not Control control) return;
                    control.DataContext = x;
                    DialogContent.Children.Add(control);
                }).DisposeWith(disposable);
        });
    }
}