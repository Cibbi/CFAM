using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.Mains;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Mains;

public partial class HeaderedMainView : CFAMUserControl<HeaderedMainViewModel>
{
    private List<Control> _overlays = new();
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

            if (ViewModel is IOverlaysProvider provider)
            {
                provider.Overlays.AsObservableChangeSet()
                    .Where(x => x.Adds > 0 || x.Removes > 0)
                    .Subscribe(x =>
                    {
                        //MainGrid.Children.Remove(Overlay);
                        foreach (var overlay in _overlays)
                        {
                            MainGrid.Children.Remove(overlay);
                        }
                        //Overlay.Children.Clear();
                        if (provider.Overlays.Count <= 0) return;
                        foreach (var overlay in provider.Overlays.OrderBy(x => x.Value.ZIndex))
                        {
                            var overlayView = new OverlayView
                            {
                                DataContext = overlay.Value,
                                [Grid.RowProperty] = 0,
                                [Grid.ColumnProperty] = 0,
                                [Grid.RowSpanProperty] = 2,
                                [Grid.ColumnSpanProperty] = 2
                            };
                            MainGrid.Children.Add(overlayView);
                            _overlays.Add(overlayView);
                        }
                    }).DisposeWith(disposable);
            }
        });
    }
}