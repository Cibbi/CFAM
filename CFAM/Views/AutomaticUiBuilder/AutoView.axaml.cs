using System.Collections;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using CFAM.Attributes.AutoControl;
using CFAM.Extensions;
using CFAM.ViewModels;
using Humanizer;
using ReactiveUI;
using Splat;
using IViewLocator = CFAM.Services.IViewLocator;

namespace CFAM.Views.AutomaticUiBuilder;

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
    
    private readonly IViewLocator _viewLocator = Locator.Current.GetRequiredService<IViewLocator>();
    private readonly IDefaultViewModelMappings _defaultMappings = Locator.Current.GetRequiredService<IDefaultViewModelMappings>();

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext;

        ContentHost.Children.Clear();
        if (DataContext is null) return;
        var dataContextType = DataContext.GetType();
        if (dataContextType.IsValueType) return;
        foreach (var prop in dataContextType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                     .Where(x => (
                                     x.GetFlags().Contains(BindingFlags.Public | BindingFlags.Instance) && 
                                     x.DeclaringType == dataContextType &&
                                     x.CustomAttributes.All(y => y.AttributeType != typeof(IgnorePropertyAttribute))) || 
                                 x.CustomAttributes.Any(z => z.AttributeType == typeof(IncludePropertyAttribute))))
        {
            Type valueType = prop.PropertyType;
            var view = _viewLocator.FindView(valueType);
            // If viewModel is Found
            if (view is { } v and not AutoView)
            {
                //view.ViewModel = propViewModel;
                v.Bind(DataContextProperty, new Binding { Source = DataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
                ContentHost.Children.Add(v);
            }

            

            // If viewmodel is not one of the default mappings try to make an AutoView of the property
            if (!_defaultMappings.TryGetMappingFor(valueType, out var defaultType))
            {
                Expander ex = new Expander(){Margin = new Thickness(6), Header = new Label{Content = prop.Name.Humanize(LetterCasing.Title)}};
                //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
                CompositeDisposable disposable = new CompositeDisposable();
                ex.WhenAnyValue(x => x.IsExpanded).Where(x => x)
                    .Subscribe(x =>
                    {
                        //if value is an IEnumerable, treat it as a list of items
                        if (valueType.IsAssignableTo(typeof(IEnumerable)))
                        {
                            var av = new AutoView();
                            av.Bind(DataContextProperty,
                                new Binding {Source = DataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                            ex.Content = av;
                            disposable.Dispose();
                        }
                        else
                        {
                            var av = new AutoView();
                            av.Bind(DataContextProperty,
                                new Binding {Source = DataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                            ex.Content = av;
                            disposable.Dispose();
                        }
                    }).DisposeWith(disposable);
                ContentHost.Children.Add(ex);
                
                continue;
            }
            view = (Control)Activator.CreateInstance(defaultType!)!;
            // 
            if (view is not { } vd) continue;
            Type typeToInstantiate = typeof(PropertyValueViewModel<>);
            Type specificType = typeToInstantiate.MakeGenericType(prop.PropertyType);
            var propViewModel = Activator.CreateInstance(specificType, prop, DataContext);
            view.DataContext = propViewModel;
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