using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reflection;
using Avalonia;
using CFAM.Attributes.AutoControl;
using Humanizer;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace CFAM.ViewModels;

public sealed class PropertyValueViewModel<T> : ViewModelBase, IActivatableViewModel
{

    private readonly object _parentObject;
    private readonly PropertyInfo _propertyInfo;

    private INotifyPropertyChanged? _parentChanged;
    private INotifyPropertyChanging? _parentChanging;
    
    public string Name { get; }

    public T? Value
    {
        get => (T?) _propertyInfo.GetValue(_parentObject);
        set
        {
            if (!EqualityComparer<object>.Default.Equals(value, _propertyInfo.GetValue(_parentObject)))
            {
                NotifyPropertyChanging(nameof(Value));
                _propertyInfo.SetValue(_parentObject, value);
                NotifyPropertyChanged(nameof(Value));
            }
        }
    }
    
    public PropertyValueViewModel(PropertyInfo propertyInfo, object parentObject)
    {
        _parentObject = parentObject;
        _propertyInfo = propertyInfo;

        var attribute = _propertyInfo.GetCustomAttribute<VisualNameAttribute>();

        Name = attribute is null ? _propertyInfo.Name.Humanize(LetterCasing.Title) : attribute.Name;

        if (_parentObject is INotifyPropertyChanged notifyChanged)
        {
            _parentChanged = notifyChanged;
        }
        
        if (_parentObject is INotifyPropertyChanging notifyChanging)
        {
            _parentChanging = notifyChanging;
        }

        this.WhenActivated(disposable =>
        {
            if(_parentChanged is not null)
            {
                _parentChanged.Events().PropertyChanged
                    .Subscribe(x => NotifyPropertyChanged(nameof(Value)))
                    .DisposeWith(disposable);
                
            }

            if (_parentChanging is not null)
            {
                _parentChanging.Events().PropertyChanging
                    .Subscribe(x => NotifyPropertyChanging(nameof(Value)))
                    .DisposeWith(disposable);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();
}