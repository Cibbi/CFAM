using System.Reflection;

namespace Cibbi.CFAM.Extensions;

public static class ReflectionExtensions
{

    private static readonly Dictionary<MemberInfo, BindingFlags> cache =
        new Dictionary<MemberInfo, BindingFlags>();
    private static readonly BindingFlags flagsFlags =
        BindingFlags.Instance | BindingFlags.NonPublic;

    public static BindingFlags GetFlags(this MemberInfo memberInfo)
    {
        if (cache.TryGetValue(memberInfo, out var bindingFlags))
            return bindingFlags;

        return cache[memberInfo] =
            (BindingFlags)memberInfo.GetType()
                .GetProperty("BindingFlags", flagsFlags)
                !.GetValue(memberInfo)!;
        
    }
    
    public static bool Contains(this BindingFlags flags, BindingFlags bindingFlags) =>
        (flags & bindingFlags) == bindingFlags;

    public static bool MatchesExactly(this BindingFlags flags, BindingFlags bindingFlags) =>
        flags == bindingFlags;

    public static bool MatchesPartly(this BindingFlags flags, BindingFlags bindingFlags) =>
        (flags & bindingFlags) != 0;
    
}