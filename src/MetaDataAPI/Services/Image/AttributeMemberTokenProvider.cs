using HandlebarsDotNet;
using System.Reflection;
using MetaDataAPI.Providers.Image;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Services.Image;

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
                var attr = p.GetCustomAttribute<HandlebarsTokenAttribute>();
                return attr != null && attr.Order == order;
            });

        if (property == null || property.GetValue(instance) is not HandlebarsToken token)
            return false;

        switch (alias)
        {
            case var _ when alias.StartsWith("TokenName-"):
                value = token.TokenName;
                break;
            case var _ when alias.StartsWith("TokenLabel-"):
                value = token.Label;
                break;
            case var _ when alias.StartsWith("TokenValue-"):
                value = token.Value;
                break;
            default:
                return false;
        }
        return true;
    }
}