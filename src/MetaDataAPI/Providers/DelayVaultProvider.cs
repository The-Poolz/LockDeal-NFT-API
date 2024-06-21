using MetaDataAPI.Services.ChainsInfo;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider : AbstractProvider
{
    public DelayVaultProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    { }

    protected override string DescriptionTemplate =>
        "The DelayVaultProvider manages the locking of {{LeftAmount}} tokens {{Erc20Token}} for leaderboard purposes. " +
        "While tokens are locked, users accumulate leaderboard points.";
}