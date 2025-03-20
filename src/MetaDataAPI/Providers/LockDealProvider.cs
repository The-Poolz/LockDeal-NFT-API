using System.Numerics;
using MetaDataAPI.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Services.Image.Handlebar;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class LockDealProvider : DealProvider
{
    [Erc721MetadataItem("start time", DisplayType.Date)]
    public BigInteger StartTime { get; }

    public LockDealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        StartTime = PoolInfo.Params[1];
    }
    public override HandlebarsImageSource ImageSource => new(
        PoolId,
        Name,
        new HandlebarsToken(Erc20Token.Name, "Left Amount", LeftAmount),
        firstLabel: new HandlebarsLabel("Start time", StartTime.DateTimeStringFormat())
    );

    protected override string DescriptionTemplate =>
        "This NFT securely locks {{LeftAmount}} units of the asset {{Erc20Token}}. " +
        "Access to these assets will commence on the designated start time of {{QueryString_StartTime}}.";
}