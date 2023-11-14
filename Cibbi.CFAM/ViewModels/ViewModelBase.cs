using System.Reactive;
using System.Reactive.Linq;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Cibbi.CFAM.ViewModels;

public abstract partial class ViewModelBase : ReactiveObject
{
    private WindowBaseViewModel? _rootViewModel;
    
    internal Func<WindowBaseViewModel?>? GetRootViewModel { get; set; }
    
    public WindowBaseViewModel? RootViewModel
    {
        get
        {
            if (_rootViewModel is null && GetRootViewModel is not null)
            {
                _rootViewModel = GetRootViewModel.Invoke();
            }
            return _rootViewModel;
        }
    }

    // Used as a target for the NotifyPropertyChanged generator
    protected virtual void NotifyPropertyChanged(string? propertyName = null)
    {
        this.RaisePropertyChanged(propertyName);
    }

    // Used as a target for the NotifyPropertyChanged generator
    protected virtual void NotifyPropertyChanging(string? propertyName = null)
    {
        this.RaisePropertyChanging(propertyName);
    }

    private void OnRootViewModelChanged()
    {
        OnRootViewModelSet();
    }

    protected virtual void OnRootViewModelSet()
    {
        
    }

}