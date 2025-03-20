using HandlebarsDotNet;
using System.Reflection;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.Image.Handlebar;

public class AttributeMemberAliasProvider : IMemberAliasProvider
{
    public bool TryGetMemberByAlias(object instance, Type targetType, ChainSegment memberAlias, [MaybeNullWhen(false)] out object value)
    {
        var property = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(prop =>
            {
                var aliasAttr = prop.GetCustomAttribute<HandlebarsMemberAttribute>();
                return aliasAttr != null && aliasAttr.Alias == memberAlias.TrimmedValue;
            });

        if (property != null)
        {
            value = property.GetValue(instance)!;
            return true;
        }

        value = null;
        return false;
    }
}