using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Image.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers.PoolInformation;

public class DelayVaultPoolInfo : PoolInfo
{
    public DelayVaultPoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : base(poolsInfo, erc20)
    { }

    public override string DescriptionTemplate =>
        "The DelayVaultProvider manages the locking of {{LeftAmount}} tokens {{Erc20Token}} for leaderboard purposes. " +
        "While tokens are locked, users accumulate leaderboard points.";
    
    public override IEnumerable<PropertyInfo> UrlifyProperties => Enumerable.Empty<PropertyInfo>();
}