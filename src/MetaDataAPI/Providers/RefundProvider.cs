using System.Numerics;
using MetaDataAPI.Providers.Image;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Services.Image.Handlebar;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class RefundProvider : AbstractProvider
{
    public AbstractProvider SubProvider { get; }
    public CollateralProvider CollateralProvider { get; }

    [Erc721MetadataItem("main coin amount", DisplayType.Number)]
    public decimal MainCoinAmount => SubProvider.LeftAmount * CollateralProvider.Rate;

    [Erc721MetadataItem("main coin collection", DisplayType.Number)]
    public BigInteger MainCoinCollection => CollateralProvider.MainCoinCollection;

    [Erc721MetadataItem("sub provider name", DisplayType.String)]
    public string SubProviderName => SubProvider.Name;

    [HandlebarsMember(order: 2)]
    public HandlebarsToken MainCoin { get; }

    public RefundProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        SubProvider = CreateFromPoolInfo(new []{ poolsInfo[1] }, chainInfo, serviceProvider);
        CollateralProvider = new CollateralProvider(poolsInfo[2], chainInfo, serviceProvider);

        MainCoin = new HandlebarsToken(CollateralProvider.MainCoin.Name, MainCoinAmount, $"Or refund of {CollateralProvider.MainCoin.Name}");
    }

    protected override string DescriptionTemplate =>
        "This NFT encompasses {{LeftAmount}} units of the asset {{Erc20Token}} " +
        "with an associated refund rate of {{CollateralProvider.Rate}}. Post rate calculation, the refundable " +
        "amount in the primary asset {{CollateralProvider.MainCoin}} will be {{MainCoinAmount}}.";
}