using System.Numerics;
using MetaDataAPI.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class LockDealProvider : DealProvider
{
    [Erc721MetadataItem("start time", DisplayType.Date)]
    public BigInteger StartTime { get; }

    public string String_StartTime => StartTime.DateTimeStringFormat();
    public string StringLabel_StartTime => "Start time";

    public LockDealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        StartTime = PoolInfo.Params[1];
    }

    protected override string DescriptionTemplate =>
        "This NFT securely locks {{LeftAmount}} units of the asset {{Erc20Token}}. " +
        "Access to these assets will commence on the designated start time of {{String_StartTime}}.";
}