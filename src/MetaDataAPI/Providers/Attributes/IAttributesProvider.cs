using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers.Attributes;

public interface IAttributesProvider
{
    public IEnumerable<Erc721Attribute> Attributes(PoolInfo poolInfo);
}