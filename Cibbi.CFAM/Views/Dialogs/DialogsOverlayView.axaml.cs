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
    }
}