namespace Cibbi.CFAM.ViewModels;

public interface IOverlaysProvider
{
    public Dictionary<string, OverlayViewModel> Overlays { get; }
}