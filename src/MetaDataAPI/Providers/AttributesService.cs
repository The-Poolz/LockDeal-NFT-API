using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public static class AttributesService
{
    public static IEnumerable<Erc721Attribute> GetProviderAttributes(BigInteger poolId)
    {
        var metadata = RpcCaller.GetMetadata(poolId);
        var parser = new MetadataParser(metadata);

        var provider = ProviderFactory.Create(parser.GetProviderAddress(), poolId);
        return provider.GetAttributes(parser.GetProviderParameters().ToArray());
    }
}