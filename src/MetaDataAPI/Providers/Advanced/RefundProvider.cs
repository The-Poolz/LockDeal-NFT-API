using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }

    public RefundProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        Attributes = new List<Erc721Attribute>
        {
            new("Rate", new ConvertWei(18).WeiToEth(basePoolInfo.Params[1]), DisplayType.Number),
            AttributesService.GetMainCoinAttribute(PoolInfo.PoolId),
            AttributesService.GetTokenAttribute(PoolInfo.PoolId)
        };
        Attributes.AddRange(ProviderFactory.Create(PoolInfo.PoolId + 1).Attributes);
    }

    public string GetDescription()
    {
        var dealProviderAttributes = ProviderFactory.Create(PoolInfo.PoolId + 3).Attributes;
        var mainCoin = Attributes[1].Value;
        var mainCoinAmountCalc = Attributes[3].Value; // not sure

        return $"This NFT encompasses {dealProviderAttributes[0].Value} units of the asset {PoolInfo.Token} with an associated refund rate of {mainCoin}. Post rate calculation, the refundable amount in the primary asset {mainCoin} will be {mainCoinAmountCalc}.";
    }
}