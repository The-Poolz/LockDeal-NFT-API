using System.ComponentModel;
using System.Reflection;
using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers.Attributes;

public class AttributesProvider : IAttributesProvider
{
    public IEnumerable<Erc721Attribute> Attributes(PoolInfo poolInfo)
    {
        var attributes = new List<Erc721Attribute>();

        var properties = poolInfo
            .GetType()
            .GetProperties()
            .Where(x => x.GetCustomAttribute<Erc721AttributeAttribute>() != null);

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<Erc721AttributeAttribute>()!;

            var value = property.GetValue(poolInfo) ??
                throw new InvalidOperationException($"Cannot process Erc721Attribute property '{property.Name}' with nullable value.");

            if (attribute.ConvertType != null)
            {
                var converter = TypeDescriptor.GetConverter(attribute.ConvertType);

                value = converter.ConvertFrom(value) ??
                    throw new InvalidOperationException($"Cannot convert Erc721Attribute property '{property.Name}' to '{attribute.ConvertType.Name}' type.");
            }

            attributes.Add(new Erc721Attribute(attribute.TraitType, value, attribute.DisplayType));
        }

        return attributes;
    }
}