using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    private readonly byte decimals;
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;
    public ProviderName Name => ProviderName.Refund;

    public RefundProvider(BigInteger poolId, byte decimals)
    {
        this.poolId = poolId;
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>
        {
            new("Rate", new ConvertWei(18).WeiToEth(values[1]), DisplayType.Number),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId)
        };
        attributes.AddRange(AttributesService.GetProviderAttributes(poolId + 1, decimals));

        return attributes;
    }
}