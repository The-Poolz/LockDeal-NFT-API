using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : IProvider
{
    public List<Erc721Attribute> Attributes => new()
    {
        new Erc721Attribute("LeftAmount", LeftAmount, DisplayType.Number),
        new Erc721Attribute("StartAmount", StartAmount, DisplayType.Number),
        new Erc721Attribute("StartTime", StartTime, DisplayType.Date),
        new Erc721Attribute("FinishTime", FinishTime, DisplayType.Date),
    };
    public BasePoolInfo PoolInfo { get; }
    public decimal LeftAmount { get; }
    public decimal StartAmount { get; }
    public uint StartTime { get; }
    public uint FinishTime { get; }

    public TimedDealProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = (uint)new ConvertWei(0).WeiToEth(basePoolInfo.Params[1]);
        FinishTime = (uint)new ConvertWei(0).WeiToEth(basePoolInfo.Params[2]);
        StartAmount = converter.WeiToEth(basePoolInfo.Params[3]);
    }

    public string GetDescription() =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}. Withdrawals are permitted in a linear fashion beginning at {TimeUtils.FromUnixTimestamp(StartTime)}, culminating in full access at {TimeUtils.FromUnixTimestamp(FinishTime)}.";
}