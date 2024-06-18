using MetaDataAPI.Erc20Manager;
using MetaDataAPI.BlockchainManager.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider : AbstractProvider
{
    public DelayVaultProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : this(poolsInfo, chainInfo, new Erc20Provider())
    { }

    public DelayVaultProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    { }

    protected override string DescriptionTemplate =>
        "The DelayVaultProvider manages the locking of {{LeftAmount}} tokens {{Erc20Token}} for leaderboard purposes. " +
        "While tokens are locked, users accumulate leaderboard points.";
}