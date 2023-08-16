using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 1;

    public BundleProvider(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>();

        for (var id = poolId; id < values[0]; id++)
        {
            var metadata = RpcCaller.GetMetadata(values[0]);
            var parser = new MetadataParser(metadata);

            var provider = ProviderFactory.Create(parser.GetProviderAddress(), poolId);
            attributes.AddRange(provider.GetAttributes(parser.GetProviderParameters().ToArray()));
        }

        return attributes;
    }
}