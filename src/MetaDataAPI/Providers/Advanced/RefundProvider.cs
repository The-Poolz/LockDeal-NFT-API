using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;

namespace MetaDataAPI.Providers;

public class RefundProvider : DealProvider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description
    {
        get
        {
            return $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
                $"with an associated refund rate of {Rate}. Post rate calculation, the refundable " +
                $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";
        }
    }
    public Provider SubProvider { get; }
    public CollateralProvider CollateralProvider { get; }
    [Display(DisplayType.Number)]
    public decimal Rate => new ConvertWei(18).WeiToEth(PoolInfo.Params[2]);
    [Display(DisplayType.Number)]
    public decimal MainCoinAmount => SubProvider.LeftAmount * Rate;
    [Display(DisplayType.String)]
    public string MainCoinName => CollateralProvider.MainCoin.Name;
    [Display(DisplayType.String)]
    public string MainCoinAddress => CollateralProvider.MainCoin.Address;
    public RefundProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = basePoolInfo.Factory.Create(PoolInfo.PoolId + 1);
        CollateralProvider = basePoolInfo.Factory.Create<CollateralProvider>(basePoolInfo.Params[1]);
    }
}