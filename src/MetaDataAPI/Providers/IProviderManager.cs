using System.Numerics;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers;

public interface IProviderManager
{
    public Erc721Metadata Metadata(BigInteger poolId, ChainInfo chainInfo);
}