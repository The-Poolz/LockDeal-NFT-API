using MetaDataAPI.Models;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Providers.PoolInfo;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : LockDealProvider
{
    [Display(DisplayType.Number)]
    public decimal StartAmount => PoolInfo.StartAmount;

    [Display(DisplayType.Date)]
    public uint FinishTime => PoolInfo.FinishTime;

    public override string ProviderName => nameof(TimedDealProvider);
    public override string Description =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}." +
        $" Withdrawals are permitted in a linear fashion beginning at {PoolInfo.StartDateTime}, culminating in full access at {PoolInfo.FinishDateTime}.";

    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo,new List<Erc721Attribute>
        {
            new("StartAmount", StartAmount),
            new("FinishTime", FinishTime, DisplayType.Date),
            new("StartTime", StartTime, DisplayType.Date),
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        })
    };

    public override TimedDealPoolInfo PoolInfo { get; }

    public TimedDealProvider(TimedDealPoolInfo poolInfo)
        : base(poolInfo)
    {
        PoolInfo = poolInfo;
    }

    public override string ToString() => $"governs a time-locked pool containing {LeftAmount}" +
        $" beginning at {PoolInfo.StartDateTime}, culminating in full access at {PoolInfo.FinishDateTime}";
}