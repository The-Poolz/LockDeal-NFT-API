using MetaDataAPI.Providers.Attributes.Models;
using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Attributes;

public class AttributesProvider : IAttributesProvider
{
    public IEnumerable<Erc721Attribute> Attributes(PoolInfo poolInfo)
    {
        throw new NotImplementedException();
    }
}