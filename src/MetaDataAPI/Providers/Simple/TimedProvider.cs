using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    public List<Erc721Attribute> Attributes => new()
    {
        new("LeftAmount", LeftAmount, DisplayType.Number),
        new("StartAmount", StartAmount, DisplayType.Number),
        new("StartTime", StartTime, DisplayType.Date),
        new("FinishTime", FinishTime, DisplayType.Date),
    };
    public BasePoolInfo PoolInfo { get; }
    public decimal LeftAmount { get; }
    public decimal StartAmount { get; }
    public long StartTime { get; }
    public long FinishTime { get; }
    public TimedProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = Convert.ToInt64(basePoolInfo.Params[1]);
        FinishTime = Convert.ToInt64(basePoolInfo.Params[2]);
        StartAmount = converter.WeiToEth(basePoolInfo.Params[3]);   
    }

    public string GetDescription() =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}. Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(StartTime)}, culminating in full access at {TimeUtils.FromUnixTimestamp(FinishTime)}.";
}