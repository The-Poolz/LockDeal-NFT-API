using HandlebarsDotNet;
using System.Reflection;
using MetaDataAPI.Providers.Image;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.Image.Handlebar;

public class AttributeMemberTokenProvider : IMemberAliasProvider
{
    public bool TryGetMemberByAlias(object instance, Type targetType, ChainSegment memberAlias, [MaybeNullWhen(false)] out object value)
    {
        value = null;
        var alias = memberAlias.TrimmedValue;
        if (!alias.StartsWith("TokenName-") && !alias.StartsWith("TokenLabel-") && !alias.StartsWith("TokenValue-"))
            return false;

        var parts = alias.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var order))
            return false;

        var property = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p =>
            {
                var attr = p.GetCustomAttribute<HandlebarsMemberAttribute>();
                return attr != null && attr.Order == order;
            });

        if (property == null || property.GetValue(instance) is not HandlebarsToken token)
            return false;

        if (alias.StartsWith("TokenName-"))
            value = token.TokenName;
        else if (alias.StartsWith("TokenLabel-"))
            value = token.Label;
        else if (alias.StartsWith("TokenValue-"))
            value = token.Value;
        else
            return false;

        return true;
    }
}