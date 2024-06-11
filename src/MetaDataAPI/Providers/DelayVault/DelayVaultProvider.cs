using MetaDataAPI.ImageGeneration.UrlifyModels;
using MetaDataAPI.ImageGeneration.UrlifyModels.DelayVault;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider : Provider
{
    public override string ProviderName => nameof(DelayVaultProvider);
    public override string Description => $"The DelayVaultProvider manages the locking of {LeftAmount} tokens {PoolInfo.Token} for leaderboard purposes. While tokens are locked, users accumulate leaderboard points.";

    public override BaseUrlifyModel Urlify => new DelayVaultUrlifyModel(PoolInfo);

    public DelayVaultProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    { }
}