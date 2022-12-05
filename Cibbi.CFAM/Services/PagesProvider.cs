using Cibbi.CFAM.Attributes;

namespace Cibbi.CFAM.Services;

public class PagesProvider
{
    private Dictionary<string,List<Page>> _pages;

    public PagesProvider()
    {
        _pages = new Dictionary<string, List<Page>>();
        
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Select(x => new {Type = x, Attribute = x.GetCustomAttributes(typeof(PageAttribute), false).FirstOrDefault() as PageAttribute})
            .Where(x => x.Attribute is not null)
            .OrderByDescending(x => x.Attribute!.Priority)
            .ThenBy(x => x.Attribute!.Path)
            .ToList();

        foreach (var type in types)
        {
            var newPage = new Page(type.Type, type.Attribute!.Path, type.Attribute.Icon);
            if (_pages.TryGetValue(type.Attribute.PageListing, out var pages))
            {
                pages.Add(newPage);
            }
            else
            {
                var newPages = new List<Page>();
                newPages.Add(newPage);
                _pages.Add(type.Attribute.PageListing, newPages);
            }
        }
    }

    public IEnumerable<Page> GetPages(string listing = "")
    {
        return _pages.TryGetValue(listing, out var pages) ? pages : Enumerable.Empty<Page>();
    }
}

/*public static class PageExtensions
{
    public static IList<Page> AddPageOfType<T>(this IList<Page> pages, string name, string iconName) where T : RoutableViewModel
    {
        pages.Add(new Page(typeof(T), name, iconName));
        return pages;
    }
}*/