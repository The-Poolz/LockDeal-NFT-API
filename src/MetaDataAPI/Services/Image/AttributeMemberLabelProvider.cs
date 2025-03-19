using HandlebarsDotNet;
using System.Reflection;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Services.Image;

public class AttributeMemberLabelProvider : IMemberAliasProvider
{
    public bool TryGetMemberByAlias(object instance, Type targetType, ChainSegment memberAlias, [MaybeNullWhen(false)] out object value)
    {
        value = null;
        var alias = memberAlias.TrimmedValue;
        if (!alias.StartsWith("LabelName-") && !alias.StartsWith("LabelValue-"))
            return false;

        var parts = alias.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var order))
            return false;

        var property = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p =>
            {
                var attr = p.GetCustomAttribute<HandlebarsLabelAttribute>();
                return attr != null && attr.Order == order;
            });

        if (property == null)
            return false;

        var labelAttr = property.GetCustomAttribute<HandlebarsLabelAttribute>()!;

        if (alias.StartsWith("LabelName-"))
        {
            value = labelAttr.LabelName;
            return true;
        }

        if (alias.StartsWith("LabelValue-"))
        {
            value = property.GetValue(instance)!;
            return true;
        }

        return false;
    }
}