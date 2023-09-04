using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }

    public RefundProvider(BigInteger poolId, byte decimals, BigInteger[] values)
    {
        Attributes = new List<Erc721Attribute>
        {
            new("Rate", new ConvertWei(18).WeiToEth(values[1]), DisplayType.Number),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId)
        };
        Attributes.AddRange(AttributesService.GetProviderAttributes(poolId + 1, decimals));
    }
}