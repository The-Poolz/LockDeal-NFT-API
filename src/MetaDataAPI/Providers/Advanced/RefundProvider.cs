using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using System.ComponentModel.DataAnnotations;
using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    private readonly byte decimals;
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;

    public RefundProvider(BigInteger poolId, byte decimals)
    {
        this.poolId = poolId;
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var converter = new ConvertWei(18);
        var attributes = new List<Erc721Attribute>
        {
            new("Rate", converter.WeiToEth(values[1]), DisplayType.Number),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId)
        };
        attributes.AddRange(AttributesService.GetProviderAttributes(poolId + 1, decimals));

        return attributes;
    }
}