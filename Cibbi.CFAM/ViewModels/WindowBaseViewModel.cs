using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public abstract partial class WindowBaseViewModel : ViewModelBase
{
    [Notify] private string _windowName = "CFAM Window";
    [Notify] private float _windowWidth = 1280;
    [Notify] private float _windowHeight = 750;
    public RoutingState? WindowRouter { get; protected set; }
}