using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IAdvancedProvider
{
    public ProviderName Name => ProviderName.Bundle;
    public BigInteger PoolId { get; }

    public BundleProvider(BigInteger poolId)
    {
        PoolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var bundleAttributes = new List<Erc721Attribute>
        {
            new("LastSubPoolId", values[0], "number")
        };

        var lastSubPoolId = (int)values[0];

        for (var poolId = PoolId; poolId < lastSubPoolId; poolId++)
        {
            var metadata = RpcCaller.GetMetadata(poolId);
            var parser = new MetadataParser(metadata);

            var provider = ProviderFactory.Create(parser.GetProviderAddress(), poolId);
            var attributes = provider.GetAttributes(parser.GetProviderParameters().ToArray());
            bundleAttributes.AddRange(attributes);
        }

        return bundleAttributes;
    }
}