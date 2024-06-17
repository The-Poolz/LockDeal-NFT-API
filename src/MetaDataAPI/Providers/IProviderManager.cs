using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers;

public interface IProviderManager
{
    public Erc721Metadata Metadata(PoolInfo poolInfo);
}