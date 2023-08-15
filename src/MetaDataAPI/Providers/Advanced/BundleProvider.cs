using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    public byte ParametersCount => 1;

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var metadata = RpcCaller.GetMetadata(values[0]);
        var parser = new MetadataParser(metadata);

        var provider = ProviderFactory.Create(parser.GetProviderAddress(), values[0]);
        var attributes = provider.GetAttributes(parser.GetProviderParameters().ToArray());

        return attributes;
    }
}