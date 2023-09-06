using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }

    public RefundProvider(BasePoolInfo basePoolInfo)
    {
        poolId = basePoolInfo.PoolId;
        Attributes = new List<Erc721Attribute>
        {
            new("Rate", new ConvertWei(18).WeiToEth(basePoolInfo.Params[1]), DisplayType.Number),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId)
        };
        Attributes.AddRange(AttributesService.GetProviderAttributes(poolId + 1));
    }

    public string GetDescription(string token)
    {
        var dealProviderAttributes = AttributesService.GetProviderAttributes(poolId + 3).ToArray();
        var mainCoin = Attributes[1].Value;
        var mainCoinAmountCalc = Attributes[3].Value; // not sure

        return $"This NFT encompasses {dealProviderAttributes[0].Value} units of the asset {token} with an associated refund rate of {mainCoin}. Post rate calculation, the refundable amount in the primary asset {mainCoin} will be {mainCoinAmountCalc}.";
    }
}