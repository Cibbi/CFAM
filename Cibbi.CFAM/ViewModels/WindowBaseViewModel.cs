using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public abstract class WindowBaseViewModel : ViewModelBase
{
    public string WindowName { get; set; } = "CFAM Window";
    public float WindowWidth { get; set; } = 1280;
    public float WindowHeight { get; set; } = 750;
    public RoutingState? WindowRouter { get; protected set; }
}