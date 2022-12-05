namespace Cibbi.CFAM.Services;

public interface IPagesProvider
{
    public IEnumerable<Page> GetPages(string listing);
}