using Cibbi.CFAM.Attributes;

namespace Cibbi.CFAM.Services;

public class PagesProvider
{
    private List<Page> _pages;

    public PagesProvider()
    {
        _pages = new List<Page>();
        
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Select(x => new {Type = x, Attribute = x.GetCustomAttributes(typeof(PageAttribute), false).FirstOrDefault() as PageAttribute})
            .Where(x => x.Attribute is not null)
            .OrderByDescending(x => x.Attribute!.Priority)
            .ThenBy(x => x.Attribute!.Path)
            .ToList();

        foreach (var type in types)
            _pages.Add(new Page(type.Type, type.Attribute!.Path, type.Attribute.Icon));
    }
    
    public IEnumerable<Page> GetPages() => _pages;
}

/*public static class PageExtensions
{
    public static IList<Page> AddPageOfType<T>(this IList<Page> pages, string name, string iconName) where T : RoutableViewModel
    {
        pages.Add(new Page(typeof(T), name, iconName));
        return pages;
    }
}*/