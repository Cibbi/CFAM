using System.Reflection;

namespace CFAM.Extensions;

public static class ReflectionExtensions
{

    private static readonly Dictionary<MemberInfo, BindingFlags> _cache =
        new Dictionary<MemberInfo, BindingFlags>();
    private static readonly BindingFlags _flagsFlags =
        BindingFlags.Instance | BindingFlags.NonPublic;

    public static BindingFlags GetFlags(this MemberInfo memberInfo)
    {
        if (_cache.TryGetValue(memberInfo, out var bindingFlags))
            return bindingFlags;

        return _cache[memberInfo] =
            (BindingFlags)memberInfo.GetType()
                .GetProperty("BindingFlags", _flagsFlags)
                !.GetValue(memberInfo)!;
        
    }
    
    public static bool Contains(this BindingFlags flags, BindingFlags bindingFlags) =>
        (flags & bindingFlags) == bindingFlags;

    public static bool MatchesExactly(this BindingFlags flags, BindingFlags bindingFlags) =>
        flags == bindingFlags;

    public static bool MatchesPartly(this BindingFlags flags, BindingFlags bindingFlags) =>
        (flags & bindingFlags) != 0;
    
}