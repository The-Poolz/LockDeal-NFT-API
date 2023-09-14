using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : Provider
{
    public override string ProviderName => nameof(TimedDealProvider);
    public override string Description =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}. Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(StartTime)}, culminating in full access at {TimeUtils.FromUnixTimestamp(FinishTime)}.";
    public override IEnumerable<Erc721Attribute> ProviderAttributes => new Erc721Attribute[]
    {
        new("LeftAmount", LeftAmount, DisplayType.Number),
        new("StartAmount", StartAmount, DisplayType.Number),
        new("StartTime", StartTime, DisplayType.Date),
        new("FinishTime", FinishTime, DisplayType.Date)
    };

    public decimal LeftAmount { get; }
    public decimal StartAmount { get; }
    public uint StartTime { get; }
    public uint FinishTime { get; }

    public TimedDealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = (uint)basePoolInfo.Params[1];
        FinishTime = (uint)basePoolInfo.Params[2];
        StartAmount = converter.WeiToEth(basePoolInfo.Params[3]);
    }
}