using System.Numerics;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class RefundProvider : AbstractProvider
{
    public AbstractProvider SubProvider { get; }

    public CollateralProvider CollateralProvider { get; }

    [Erc721Attribute("main coin amount", DisplayType.Number)]
    public decimal MainCoinAmount => SubProvider.LeftAmount * CollateralProvider.Rate;

    [Erc721Attribute("main coin collection", DisplayType.Number)]
    public BigInteger MainCoinCollection => CollateralProvider.MainCoinCollection;

    [Erc721Attribute("sub provider name", DisplayType.String)]
    public string SubProviderName => SubProvider.Name;

    public RefundProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    {
        SubProvider = AbstractProvider.CreateFromPoolInfo(new []{ poolsInfo[1] }, chainInfo, erc20Provider);
        CollateralProvider = new CollateralProvider(poolsInfo[2], chainInfo, erc20Provider);
    }

    protected override string DescriptionTemplate =>
        "This NFT encompasses {{LeftAmount}} units of the asset {{Erc20Token}} " +
        "with an associated refund rate of {{CollateralProvider.Rate}}. Post rate calculation, the refundable " +
        "amount in the primary asset {{CollateralProvider.MainCoin}} will be {{MainCoinAmount}}.";
}