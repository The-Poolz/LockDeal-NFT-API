using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Services.Image.Handlebar;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider : AbstractProvider
{
    public DelayVaultProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    { }

    public override HandlebarsImageSource ImageSource => new(
        PoolId,
        Name,
        new HandlebarsToken(Erc20Token.Name, "Left Amount", LeftAmount)
    );

    protected override string DescriptionTemplate =>
        "The DelayVaultProvider manages the locking of {{LeftAmount}} tokens {{Erc20Token}} for leaderboard purposes. " +
        "While tokens are locked, users accumulate leaderboard points.";
}