using System.Numerics;
using MetaDataAPI.Models;
using MetaDataAPI.Extensions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.ImageGeneration.UrlifyModels;
using MetaDataAPI.ImageGeneration.UrlifyModels.Advanced;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description =>
        $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
        $"with an associated refund rate of {Rate}. Post rate calculation, the refundable " +
        $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";

    public override BaseUrlifyModel Urlify => new RefundUrlifyModel(PoolInfo);

    public Provider SubProvider { get; }

    public CollateralProvider CollateralProvider { get; }

    [Display(DisplayType.Number)]
    public decimal Rate => PoolInfo.Params[2].WeiToEth(21);

    [Display(DisplayType.Number)]
    public decimal MainCoinAmount => SubProvider.LeftAmount * Rate;

    [Display(DisplayType.Number)]
    public BigInteger MainCoinCollection => CollateralProvider.MainCoinCollection;

    [Display(DisplayType.String)]
    public string SubProviderName => SubProvider.ProviderName;

    public RefundProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = ProviderFactory.Create(new[] { basePoolInfo[1] });
        CollateralProvider = new CollateralProvider(basePoolInfo[2]);
    }
}