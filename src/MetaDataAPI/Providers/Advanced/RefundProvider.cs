using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;

    public RefundProvider(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>
        {
            new("RateToWei", values[1], "number"),
        };
        attributes.AddRange(AttributesService.GetProviderAttributes(poolId));
        attributes.AddRange(AttributesService.GetProviderAttributes(values[0]));

        return attributes;
    }
}