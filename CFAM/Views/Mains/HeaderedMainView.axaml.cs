using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using CFAM.Behaviors.WindowHandlers;
using CFAM.ViewModels.Mains;
using ReactiveUI;

namespace CFAM.Views.Mains;

public partial class HeaderedMainView : UserControl, IActivatableView
{
    public HeaderedMainView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposable =>
        {
            if (DataContext is not HeaderedMainViewModel ViewModel) return;
            ViewModel.Router.NavigationChanged.Subscribe(_ =>
            {
                var x = ViewModel.Router.GetCurrentViewModel();
                if(x is null) return;
                MainContent.Children.Clear();
                if (ViewModel.ViewLocator.FindView(x) is not { } control) return;
                control.DataContext = x;
                MainContent.Children.Add(control);
                
                if (ViewModel.Router.NavigationStack.Count > 1)
                    BackButton.Classes.Add("visible");
                else
                    BackButton.Classes.Remove("visible");

            }).DisposeWith(disposable);
        });
    }
    
    private void GetBehaviorsFromControl()
    {
        // Get the behaviors collection for this control
        var behaviors = Interaction.GetBehaviors(this);
    
        // Now you can access individual behaviors
        var overlaysHandler = behaviors.OfType<OverlaysHandler>().FirstOrDefault();
        var dialogsHandler = behaviors.OfType<DialogsHandler>().FirstOrDefault();
    
        // Connect them programmatically
        if (overlaysHandler != null && dialogsHandler != null)
        {
            dialogsHandler.OverlaysHandler = overlaysHandler;
        }
    }

}