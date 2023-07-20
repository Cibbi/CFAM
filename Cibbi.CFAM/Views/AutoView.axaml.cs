using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Cibbi.CFAM.Attributes;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.ViewModels;
using Humanizer;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.Views;

public partial class AutoView : UserControl, IViewFor
{
    public static readonly StyledProperty<object?> ViewModelProperty = AvaloniaProperty
        .Register<ReactiveCoreWindow<object>, object?>(nameof(ViewModel));
    
    public object? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    
    public AutoView()
    {
        InitializeComponent();
        this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);

    }
    
    private IViewLocator _viewLocator = Locator.Current.GetRequiredService<IViewLocator>();

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext;

        ContentHost.Children.Clear();
        if (DataContext is null) return;
        var dataContextType = DataContext.GetType();
        if (dataContextType.IsValueType || dataContextType == typeof(PropertyValueViewModel<>)) return;
        foreach (var prop in dataContextType.GetProperties()
                     .Where(x => (x.GetFlags().Contains(BindingFlags.Public | BindingFlags.Instance) && x.DeclaringType == dataContextType &&
                                    x.CustomAttributes.All(y => y.AttributeType != typeof(IgnorePropertyAttribute))) || 
                                 x.CustomAttributes.Any(z => z.AttributeType == typeof(IncludePropertyAttribute))))
        {
            object? value = prop.GetValue(DataContext);
            if (value is null) continue;
            var view = _viewLocator.ResolveView(value);
            // If viewModel is Found
            if (view is Control v and not AutoView)
            {
                //view.ViewModel = propViewModel;
                v.Bind(DataContextProperty, new Binding { Source = DataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
                ContentHost.Children.Add(v);
            }

            // If viewmodel is not one of the default mappings try to make an AutoView of the property
            if (!DefaultViewModelMappings.Instance.TryGetValue(value!.GetType(), out var defaultType))
            {
                
                Expander ex = new Expander(){Margin = new Thickness(6), Header = new Label{Content = prop.Name.Humanize(LetterCasing.Title)}};
                //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
                CompositeDisposable disposable = new CompositeDisposable();
                ex.WhenAnyValue(x => x.IsExpanded).Where(x => x)
                    .Subscribe(x =>
                    {
                        var av = new AutoView();
                        av.Bind(DataContextProperty,
                            new Binding {Source = DataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                        ex.Content = av;
                        disposable.Dispose();
                    }).DisposeWith(disposable);
                ContentHost.Children.Add(ex);
                
                continue;
            }
            view = (IViewFor)Activator.CreateInstance(defaultType!)!;
            // 
            if (view is not Control vd) continue;
            
            var propViewModel = Activator.CreateInstance(view.GetType().GetProperty("ViewModel")!.PropertyType, prop, DataContext);
            view.ViewModel = propViewModel;
            //v.Bind(DataContextProperty, new Binding { Source = propViewModel, Path = prop.Name, Mode = BindingMode.TwoWay});
            ContentHost.Children.Add(vd);

        }
    }

    private void OnViewModelChanged(object? value)
    {
        if (value == null)
        {
            ClearValue(DataContextProperty);
        }
        else if (DataContext != value)
        {
            DataContext = value;
        }
    }

    
}