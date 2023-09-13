using Newtonsoft.Json;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    [JsonIgnore]
    public IProvider SubProvider { get; }

    [JsonIgnore]
    public IProvider CollateralProvider { get; }

    [JsonIgnore]
    public decimal Rate { get; }

    public RefundProvider(BasePoolInfo basePoolInfo) : base(basePoolInfo)
    {
        SubProvider = basePoolInfo.Factory.Create(PoolInfo.PoolId + 1);
        CollateralProvider = basePoolInfo.Factory.Create(basePoolInfo.Params[1]);
        Rate = new ConvertWei(18).WeiToEth(basePoolInfo.Params[2]);
        AddAttributes(nameof(RefundProvider));
    }

    public override List<Erc721Attribute> GetParams()
    {
        var result = new List<Erc721Attribute>
        {
           new("Rate", Rate, DisplayType.Number),
        };
        result.AddRange(CollateralProvider.Attributes);
        result.AddRange(SubProvider.Attributes);
        return result;
    }

    public override string GetDescription()
    {
        var mainCoinAmountCalc = new ConvertWei(18).WeiToEth(CollateralProvider.PoolInfo.Params[0]) * Rate;
        return $"This NFT encompasses {Attributes[0].Value} units of the asset {PoolInfo.Token} with an associated refund rate of {Rate}. Post rate calculation, the refundable amount in the primary asset {CollateralProvider.PoolInfo.Token} will be {mainCoinAmountCalc}.";
    }
}