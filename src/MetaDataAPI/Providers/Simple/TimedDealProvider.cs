using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.DynamoDb;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : LockDealProvider
{
    public override string ToString() => $"governs a time-locked pool containing {LeftAmount}" +
        $" beginning at {StartDateTime}, culminating in full access at {FinishDateTime}";

    public override string ProviderName => nameof(TimedDealProvider);
    public override string Description =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}." +
        $" Withdrawals are permitted in a linear fashion beginning at {StartDateTime}, culminating in full access at {FinishDateTime}.";
    [Display(DisplayType.Number)]
    public decimal StartAmount { get; }
    [Display(DisplayType.Date)]
    public uint FinishTime { get; }
    public DateTime FinishDateTime => TimeUtils.FromUnixTimestamp(FinishTime);
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

    public TimedDealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        FinishTime = (uint)basePoolInfo.Params[2];
        StartAmount = converter.WeiToEth(basePoolInfo.Params[3]);
    }
}