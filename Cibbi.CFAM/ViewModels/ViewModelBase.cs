using PropertyChanged.SourceGenerator;
using ReactiveUI;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Cibbi.CFAM.ViewModels;

public abstract partial class ViewModelBase : ReactiveObject
{
    [Notify] private WindowBaseViewModel? _rootViewModel;

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