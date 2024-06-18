using System.Numerics;
using HandlebarsDotNet;
using Net.Urlify.Attributes;
using MetaDataAPI.Extensions;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

internal class LockDealProvider : DealProvider
{
    [QueryStringProperty("Start time", order: 1)]
    public string QueryString_StartTime => StartTime.DateTimeStringFormat();

    [Erc721Attribute("start time", DisplayType.Date)]
    public BigInteger StartTime { get; }

    public LockDealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    {
        StartTime = PoolInfo.Params[1];
    }

    protected override string DescriptionTemplate =>
        "This NFT securely locks {{LeftAmount}} units of the asset {{Erc20Token}}. " +
        "Access to these assets will commence on the designated start time of {{QueryString_StartTime}}.";
}