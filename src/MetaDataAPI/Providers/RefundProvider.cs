using System.Numerics;
using System.Globalization;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
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

    [Erc721MetadataItem("sub provider name")]
    public string SubProviderName => SubProvider.Name;

    public string String_MainCoinAmount => MainCoinAmount.ToString(CultureInfo.InvariantCulture);
    public string String_RefundLabel => $"Or refund of {CollateralProvider.MainCoin.Name}";

    public RefundProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        SubProvider = CreateFromPoolInfo(new []{ poolsInfo[1] }, chainInfo, serviceProvider);
        CollateralProvider = new CollateralProvider(poolsInfo[2], chainInfo, serviceProvider);
    }

    protected override string DescriptionTemplate =>
        "This NFT encompasses {{LeftAmount}} units of the asset {{Erc20Token}} " +
        "with an associated refund rate of {{CollateralProvider.Rate}}. Post rate calculation, the refundable " +
        "amount in the primary asset {{CollateralProvider.MainCoin}} will be {{MainCoinAmount}}.";
}