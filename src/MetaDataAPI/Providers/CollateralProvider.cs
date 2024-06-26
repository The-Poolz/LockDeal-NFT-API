using Nethereum.Web3;
using System.Numerics;
using Net.Urlify.Attributes;
using MetaDataAPI.Extensions;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class CollateralProvider : AbstractProvider
{
    internal enum CollateralType
    {
        MainCoinCollector = 1,
        TokenCollector = 2,
        MainCoinHolder = 3
    }

    public Erc20Token MainCoin { get; }
    public new Erc20Token Erc20Token { get; }
    public new decimal LeftAmount => base.LeftAmount;
    public new BigInteger VaultId => base.VaultId;

    [Erc721MetadataItem("main coin collection", DisplayType.Number)]
    public BigInteger MainCoinCollection => PoolInfo.VaultId;

    [Erc721MetadataItem("collection", DisplayType.Number)]
    public BigInteger Collection => SubProviders[CollateralType.TokenCollector].PoolInfo.VaultId;

    [Erc721MetadataItem("main coin collector amount", DisplayType.Number)]
    public decimal MainCoinCollectorAmount => SubProviders[CollateralType.MainCoinCollector].LeftAmount;

    [Erc721MetadataItem("token collector amount", DisplayType.Number)]
    public decimal TokenCollectorAmount => SubProviders[CollateralType.TokenCollector].LeftAmount;

    [Erc721MetadataItem("main coin holder amount", DisplayType.Number)]
    public decimal MainCoinHolderAmount => SubProviders[CollateralType.MainCoinHolder].LeftAmount;

    [Erc721MetadataItem("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    [Erc721MetadataItem("rate", DisplayType.Number)]
    public decimal Rate { get; }

    [QueryStringProperty("Finish time", order: 1)]
    public string QueryString_FinishTime => FinishTime.DateTimeStringFormat();

    internal Dictionary<CollateralType, DealProvider> SubProviders { get; }

    public CollateralProvider(BasePoolInfo poolInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : this(serviceProvider.FetchPoolInfo(poolInfo.PoolId), chainInfo, serviceProvider)
    { }

    public CollateralProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        MainCoin = Erc20Provider.GetErc20Token(chainInfo, poolsInfo[0].Token);
        Erc20Token = Erc20Provider.GetErc20Token(chainInfo, poolsInfo[2].Token);
        FinishTime = PoolInfo.Params[1];
        Rate = Web3.Convert.FromWei(PoolInfo.Params[2], 21);

        SubProviders = Enumerable.Range(1, poolsInfo.Length - 1)
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