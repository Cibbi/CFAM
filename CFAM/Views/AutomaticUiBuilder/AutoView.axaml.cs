using System.Collections;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
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
    
    internal object? ParentObject { get; set; }
    internal PropertyInfo? ParentPropertyInfo { get; set; }
    
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

        /*if (ParentObject is not null && _defaultMappings.TryGetMappingFor(dataContextType, out var dt))
        {
            
            var view = (Control)Activator.CreateInstance(dt!)!;
            // 
            if (view is { } vd)
            {
                Type typeToInstantiate = typeof(PropertyValueViewModel<>);
                Type specificType = typeToInstantiate.MakeGenericType(dataContextType);
                var propViewModel = Activator.CreateInstance(specificType, DataContext);
                view.DataContext = propViewModel;
                //v.Bind(DataContextProperty, new Binding { Source = propViewModel, Path = prop.Name, Mode = BindingMode.TwoWay});
                ContentHost.Children.Add(vd);
                return;
            }
        }*/
        
        if (dataContextType.IsValueType) return;
        foreach (var prop in dataContextType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                     .Where(x => (
                                     x.GetFlags().Contains(BindingFlags.Public | BindingFlags.Instance) && 
                                     x.DeclaringType == dataContextType &&
                                     x.CustomAttributes.All(y => y.AttributeType != typeof(IgnorePropertyAttribute))) || 
                                 x.CustomAttributes.Any(z => z.AttributeType == typeof(IncludePropertyAttribute))))
        {
            var control = GenerateView(prop, DataContext, _viewLocator, _defaultMappings);
            
            ContentHost.Children.Add(control);
        }
    }
    
    private static Control GenerateView(PropertyInfo prop, object? dataContext, IViewLocator viewLocator, IDefaultViewModelMappings defaultMappings)
    {
        Type valueType = prop.GetValue(dataContext)?.GetType() ?? prop.PropertyType;
        var view = viewLocator.FindView(valueType);
        // If viewModel is Found
        if (view is { } v and not AutoView)
        {
            //view.ViewModel = propViewModel;
            v.Bind(DataContextProperty, new Binding { Source = dataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
            //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
            return v;
        }

        // If viewmodel is not one of the default mappings try to make an AutoView of the property
        if (!defaultMappings.TryGetMappingFor(valueType, out var defaultType))
        {
            Expander ex = new()
            {
                Margin = new Thickness(6), 
                Header = new Label{Content = prop.Name.Humanize(LetterCasing.Title)},
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            
            //ContentHost.Children.Add(new Label{Content = prop.Name.Humanize(LetterCasing.Title)});
            CompositeDisposable disposable = new CompositeDisposable();
            ex.WhenAnyValue(x => x.IsExpanded).Where(x => x)
                .Subscribe(x =>
                {
                    //if value is an IEnumerable, treat it as a list of items
                    if (valueType.IsAssignableTo(typeof(IEnumerable)))
                    {
                        var av = new AutoIListView();
                        av.Bind(DataContextProperty,
                            new Binding {Source = dataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                        ex.Content = av;
                        disposable.Dispose();
                    }
                    else
                    {
                        var av = new AutoView();
                        av.Bind(DataContextProperty,
                            new Binding {Source = dataContext, Path = prop.Name, Mode = BindingMode.TwoWay});
                        ex.Content = av;
                        disposable.Dispose();
                    }
                }).DisposeWith(disposable);
            return ex;
        }
        view = (Control)Activator.CreateInstance(defaultType)!;
        // 
        if (view is not { } vd) return null;
        Type typeToInstantiate = typeof(PropertyValueViewModel<>);
        Type specificType = typeToInstantiate.MakeGenericType(valueType);
        var propViewModel = Activator.CreateInstance(specificType, prop, dataContext);
        view.DataContext = propViewModel;
        //v.Bind(DataContextProperty, new Binding { Source = propViewModel, Path = prop.Name, Mode = BindingMode.TwoWay});
        return vd;
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