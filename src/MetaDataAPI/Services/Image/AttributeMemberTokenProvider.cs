using HandlebarsDotNet;
using System.Reflection;
using MetaDataAPI.Providers.Image;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Services.Image;

public class AttributeMemberTokenProvider : IMemberAliasProvider
{
    public bool TryGetMemberByAlias(object? instance, Type targetType, ChainSegment? memberAlias, [MaybeNullWhen(false)] out object value)
    {
        value = null;
        if (instance == null || memberAlias == null || string.IsNullOrEmpty(memberAlias.TrimmedValue))
            return false;

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

        if (property == null)
            return false;

        if (alias.StartsWith("TokenName-") && property.GetValue(instance) is HandlebarsToken token1)
        {
            value = token1.TokenName;
            return true;
        }
        if (alias.StartsWith("TokenLabel-") && property.GetValue(instance) is HandlebarsToken token2)
        {
            value = token2.Label;
            return true;
        }
        if (alias.StartsWith("TokenValue-") && property.GetValue(instance) is HandlebarsToken token3)
        {
            value = token3.Value;
            return true;
        }
        return false;
    }
}