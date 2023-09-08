using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class RefundProvider : IProvider
{
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }
    public IProvider SubProvider { get; }
    public IProvider CollateralProvider { get; }
    public decimal Rate { get; }
    public RefundProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        SubProvider = basePoolInfo.Factory.FromPoolId(PoolInfo.PoolId + 1);
        CollateralProvider = basePoolInfo.Factory.FromPoolId(basePoolInfo.Params[1]);
        Rate = new ConvertWei(18).WeiToEth(basePoolInfo.Params[2]);
        Attributes = new List<Erc721Attribute>
        {
            new("Rate", Rate,DisplayType.Number)
        };
        Attributes.AddRange(CollateralProvider.Attributes);
        Attributes.AddRange(SubProvider.Attributes);
    }

    public string GetDescription()
    {
        var mainCoinAmountCalc = new ConvertWei(18).WeiToEth(CollateralProvider.PoolInfo.Params[0]) * Rate;
        return $"This NFT encompasses {Attributes[0].Value} units of the asset {PoolInfo.Token} with an associated refund rate of {Rate}. Post rate calculation, the refundable amount in the primary asset {CollateralProvider.PoolInfo.Token} will be {mainCoinAmountCalc}.";
    }
}