using MetaDataAPI.Models.DynamoDb;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider(BasePoolInfo[] basePoolInfo) : Provider(basePoolInfo)
{
    public override string ProviderName => nameof(DelayVaultProvider);
    public override string Description => $"The DelayVaultProvider manages the locking of {LeftAmount} tokens {PoolInfo.Token} for leaderboard purposes. While tokens are locked, users accumulate leaderboard points.";

    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new(ProviderName, PoolInfo,
        [
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        ])
    };
}