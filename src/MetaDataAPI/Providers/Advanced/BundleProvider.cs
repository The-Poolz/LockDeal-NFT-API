using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    public ProviderName Name => ProviderName.Bundle;
    public BigInteger PoolId { get; }

    public BundleProvider(BigInteger poolId)
    {
        PoolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params object[] values)
    {
        return new Erc721Attribute[0];
    }
}