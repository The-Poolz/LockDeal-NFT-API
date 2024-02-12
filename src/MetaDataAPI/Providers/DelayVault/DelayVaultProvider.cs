using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.Providers;

public class DelayVaultProvider : Provider
{
    public override string ProviderName => nameof(DelayVaultProvider);
    public override string Description => $"The DelayVaultProvider manages the locking of {LeftAmount} tokens {PoolInfo.Token} for leaderboard purposes. While tokens are locked, users accumulate leaderboard points.";
    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo, new List<Erc721Attribute>
        {
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        })
    };

    public DealPoolInfo PoolInfo { get; }

    public DelayVaultProvider(DealPoolInfo poolInfo)
        : base(poolInfo)
    {
        PoolInfo = poolInfo;
    }
}