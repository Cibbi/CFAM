using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Cibbi.CFAM.ViewModels.WindowServices;
using Cibbi.CFAM.Views;
using ReactiveUI;
using DynamicData;

namespace Cibbi.CFAM.Behaviors.WindowHandlers;

public class OverlaysHandler : WindowServiceHandler
{
    public static readonly StyledProperty<Panel> TargetPanelProperty =
        AvaloniaProperty.Register<OverlaysHandler, Panel>(nameof(Panel));
    
    public static readonly DirectProperty<OverlaysHandler, string?> NameProperty =
        AvaloniaProperty.RegisterDirect<OverlaysHandler, string?>(nameof(Name), o => o.Name, (o, v) => o.Name = v);

    public Panel TargetPanel
    {
        get => GetValue(TargetPanelProperty);
        set => SetValue(TargetPanelProperty, value);
    }

    public string? Name { get; set; }
    
    private readonly Overlays _overlays = new();
    private readonly List<Control> _currentOverlays = [];

    private ThrottledTask _checkPanelTask;
    private CompositeDisposable? _disposable;

    public OverlaysHandler()
    {
        _checkPanelTask = new ThrottledTask(CheckEnableState, TimeSpan.FromMicroseconds(100));
    }
    
    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        _disposable = new CompositeDisposable();
        _overlays._items.AsObservableChangeSet()
            .Where(x => x.Adds > 0 || x.Removes > 0)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                //TODO: improve logic by removing and adding only the needed overlays when updated instead of a full clear
                foreach (var overlay in _currentOverlays)
                {
                    TargetPanel.Children.Remove(overlay);
                }
                _currentOverlays.Clear();
                TargetPanel.IsVisible = false;
                if (_overlays._items.Count <= 0)
                {
                    return;
                }
                foreach (var overlay in _overlays._items.OrderBy(y => y.Value.ZIndex))
                {
                    var overlayView = new OverlayView
                    {
                        DataContext = overlay.Value,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    TargetPanel.Children.Add(overlayView);
                    _currentOverlays.Add(overlayView);
                    
                    overlay.Value.WhenAnyValue(x => x.IsEnabled)
                        .Subscribe(_ => _checkPanelTask.Run())
                        .DisposeWith(_disposable);
                }
            }).DisposeWith(_disposable);
    }

    private void CheckEnableState()
    {
        TargetPanel.IsVisible = _currentOverlays.Any(x => x.IsVisible);
    }

    protected override void OnDetachedFromVisualTree()
    {
        _disposable?.Dispose();
    }

    protected override void OnDataContextChanged()
    {
        base.OnDataContextChanged();
        Provider?.AddService(_overlays);
    }

    public Overlays GetOverlaysService()
    {
        return _overlays;
    }
}