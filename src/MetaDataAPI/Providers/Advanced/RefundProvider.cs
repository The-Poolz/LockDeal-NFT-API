using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description
    {
        get
        {
            var attributes = GetErc721Attributes().ToArray();
            var mainCoinAmountCalc = new ConvertWei(18).WeiToEth(CollateralProvider.PoolInfo.Params[0]) * Rate;
            return $"This NFT encompasses {attributes[0].Value} units of the asset {PoolInfo.Token} with an associated refund rate of {Rate}. Post rate calculation, the refundable amount in the primary asset {CollateralProvider.PoolInfo.Token} will be {mainCoinAmountCalc}.";
        }
    }
    public override IEnumerable<Erc721Attribute> ProviderAttributes
    {
        get
        {
            var result = new List<Erc721Attribute>
            {
                new("Rate", Rate, DisplayType.Number),
            };
            result.AddRange(CollateralProvider.ProviderAttributes);
            result.AddRange(SubProvider.ProviderAttributes);
            return result;
        }
    }

    public Provider SubProvider { get; }
    public Provider CollateralProvider { get; }
    public decimal Rate { get; }

    public RefundProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = basePoolInfo.Factory.Create(PoolInfo.PoolId + 1);
        CollateralProvider = basePoolInfo.Factory.Create(basePoolInfo.Params[1]);
        Rate = new ConvertWei(21).WeiToEth(basePoolInfo.Params[2]);
    }
}