using MetaDataAPI.Utils;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.Providers;

public class LockDealProvider : DealProvider
{
    public override string ProviderName => nameof(LockDealProvider);
    public override string Description =>
        $"This NFT securely locks {LeftAmount} units of the asset {PoolInfo.Token}. " +
        $"Access to these assets will commence on the designated start time of {StartDateTime}.";

    [Display(DisplayType.Date)]
    public uint StartTime => PoolInfo.StartTime;

    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo, new List<Erc721Attribute>
        {
            new("StartTime", StartTime, DisplayType.Date),
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        })
    };

    public LockDealPoolInfo PoolInfo { get; }

    public LockDealProvider(List<BasePoolInfo> basePoolInfo, string rpcUrl)
        : base(basePoolInfo, rpcUrl)
    {
        PoolInfo = new LockDealPoolInfo(basePoolInfo[0], rpcUrl);
    }

    public override string ToString() => $"securely locks {LeftAmount} units of the asset Until {StartDateTime}";
}