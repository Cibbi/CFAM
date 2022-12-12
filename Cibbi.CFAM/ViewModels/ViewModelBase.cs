using ReactiveUI;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Cibbi.CFAM.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public WindowBaseViewModel? RootViewModel { get; set; }

    // Used as a target for the NotifyPropertyChanged generator
    public virtual void NotifyPropertyChanged(string? propertyName = null)
    {
        this.RaisePropertyChanged(propertyName);
    }

    // Used as a target for the NotifyPropertyChanged generator
    public virtual void NotifyPropertyChanging(string? propertyName = null)
    {
        this.RaisePropertyChanging(propertyName);
    }
        
}