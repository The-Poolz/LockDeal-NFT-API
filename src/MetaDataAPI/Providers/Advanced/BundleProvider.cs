using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IAdvancedProvider
{
    public byte ParametersCount => 1;
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

        var lastSubPoolId = values[0];

        var metadata = RpcCaller.GetMetadata(lastSubPoolId);
        var parser = new MetadataParser(metadata);

        var provider = ProviderFactory.Create(parser.GetProviderAddress(), lastSubPoolId);
        var attributes = provider.GetAttributes(parser.GetProviderParameters().ToArray());
        bundleAttributes.AddRange(attributes);

        return bundleAttributes;
    }
}