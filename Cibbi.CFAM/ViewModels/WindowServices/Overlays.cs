namespace Cibbi.CFAM.ViewModels.WindowServices;

public class Overlays : IWindowService
{
    internal Dictionary<string, OverlayViewModel> _items { get; } = [];
    
    public OverlayViewModel? GetOverlay(string key)
    {
        return _items.GetValueOrDefault(key);
    }
    
    public bool AddOverlay(string key, OverlayViewModel overlay)
    {
        return _items.TryAdd(key, overlay);
    }
    
    public bool RemoveOverlay(string key)
    {
        return _items.Remove(key);
    }
    
    public IEnumerable<OverlayViewModel> GetAllOverlays()
    {
        return _items.Values;
    }
}