using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public override string ProviderName => nameof(DealProvider);
    public override string Description =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";
    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo, new List<Erc721Attribute>
        {
            new("Collection", Collection),
            new("LeftAmount", PoolInfo.LeftAmount)
        })
    };

    public virtual DealPoolInfo PoolInfo { get; }

    public DealProvider(DealPoolInfo poolInfo)
        : base(poolInfo)
    {
        PoolInfo = poolInfo;
    }

    public override string ToString() => $"immediate access to {LeftAmount}"; 
}