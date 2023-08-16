using System.Numerics;
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

        for (var id = poolId + 1; id <= values[0]; id++)
        {
            attributes.AddRange(AttributesService.GetProviderAttributes(id));
        }

        return attributes;
    }
}