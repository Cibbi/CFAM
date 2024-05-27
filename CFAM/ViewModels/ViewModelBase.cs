using ReactiveUI;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace CFAM.ViewModels;

public abstract partial class ViewModelBase : ReactiveObject
{
    
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