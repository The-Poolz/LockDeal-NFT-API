using Nethereum.Web3;
using System.Numerics;
using Net.Urlify.Attributes;
using MetaDataAPI.Extensions;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Services.Erc20;

namespace MetaDataAPI.Providers;

internal enum CollateralType
{
    MainCoinCollector = 1,
    TokenCollector = 2,
    MainCoinHolder = 3
}

public class CollateralProvider : AbstractProvider
{
    public Erc20Token MainCoin { get; }
    public new Erc20Token Erc20Token { get; }
    public new decimal LeftAmount => base.LeftAmount;
    public new BigInteger VaultId => base.VaultId;

    [Erc721Attribute("main coin collection", DisplayType.Number)]
    public BigInteger MainCoinCollection => PoolInfo.VaultId;

    [Erc721Attribute("collection", DisplayType.Number)]
    public BigInteger Collection => SubProvider[CollateralType.TokenCollector].PoolInfo.VaultId;

    [Erc721Attribute("main coin collector amount", DisplayType.Number)]
    public decimal MainCoinCollectorAmount => SubProvider[CollateralType.MainCoinCollector].LeftAmount;

    [Erc721Attribute("token collector amount", DisplayType.Number)]
    public decimal TokenCollectorAmount => SubProvider[CollateralType.TokenCollector].LeftAmount;

    [Erc721Attribute("main coin holder amount", DisplayType.Number)]
    public decimal MainCoinHolderAmount => SubProvider[CollateralType.MainCoinHolder].LeftAmount;

    [Erc721Attribute("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    [Erc721Attribute("rate", DisplayType.Number)]
    public decimal Rate { get; }

    [QueryStringProperty("Finish time", order: 1)]
    public string QueryString_FinishTime => FinishTime.DateTimeStringFormat();

    internal Dictionary<CollateralType, DealProvider> SubProvider { get; }

    public CollateralProvider(BasePoolInfo poolInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : this(FetchPoolInfo(poolInfo.PoolId, chainInfo), chainInfo, serviceProvider)
    { }

    public CollateralProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        MainCoin = erc20Provider.GetErc20Token(chainInfo, poolsInfo[0].Token);
        Erc20Token = erc20Provider.GetErc20Token(chainInfo, poolsInfo[2].Token);
        FinishTime = PoolInfo.Params[1];
        Rate = Web3.Convert.FromWei(PoolInfo.Params[2], 21);

        SubProvider = Enumerable.Range(1, poolsInfo.Length - 1)
            .ToDictionary(
                i => (CollateralType)i,
                i => new DealProvider(poolsInfo[i], chainInfo, serviceProvider)
            );
    }

    protected override string DescriptionTemplate =>
        "Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens {{Erc20Token}}, for Main Coin {{MainCoin}}. " +
        "It holds {{MainCoinCollectorAmount}} for the main coin collector, {{TokenCollectorAmount}} for the token collector," +
        " and {{MainCoinHolderAmount}} for the main coin holder, valid until {{QueryString_FinishTime}}.";
}