using ReactiveUI;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Cibbi.CFAM.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
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
    
    public abstract class RoutableViewModel : ViewModelBase, IRoutableViewModel
    {
        public abstract string UrlPathSegment { get; }
        public IScreen HostScreen { get; }
    
        public RoutableViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}