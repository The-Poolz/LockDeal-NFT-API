using HandlebarsDotNet;
using System.Reflection;
using HandlebarsDotNet.PathStructure;
using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.Image.Handlebar;

public class UnifiedMemberAliasProvider : IMemberAliasProvider
{
    public bool TryGetMemberByAlias(object instance, Type targetType, ChainSegment memberAlias, [MaybeNullWhen(false)] out object value)
    {
        value = null;
        var property = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(prop =>
            {
                var aliasAttr = prop.GetCustomAttribute<HandlebarsMemberAttribute>();
                return aliasAttr != null;
            });
        if (property == null) return false;

        return property.GetCustomAttribute<HandlebarsMemberAttribute>()!.MemberType switch
        {
            HandlebarsMemberType.Alias => new AttributeMemberAliasProvider().TryGetMemberByAlias(instance, targetType,memberAlias, out value),
            HandlebarsMemberType.Label => new AttributeMemberLabelProvider().TryGetMemberByAlias(instance, targetType, memberAlias, out value),
            HandlebarsMemberType.Token => new AttributeMemberTokenProvider().TryGetMemberByAlias(instance, targetType, memberAlias, out value)
        };
    }
}