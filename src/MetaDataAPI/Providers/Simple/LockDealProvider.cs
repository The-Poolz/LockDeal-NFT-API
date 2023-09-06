using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class LockDealProvider : IProvider
{
    public List<Erc721Attribute> Attributes => new()
    { new("LeftAmount", LeftAmount, DisplayType.Number),
        new("StartTime", StartTime, DisplayType.Date) };
    public BasePoolInfo PoolInfo { get; }
    public decimal LeftAmount { get; }
    public long StartTime { get; }
    public LockDealProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = (long)new ConvertWei(0).WeiToEth(basePoolInfo.Params[1]);
    }

    public string GetDescription() =>
        $"This NFT securely locks {LeftAmount} units of the asset {PoolInfo.Token}. Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(StartTime)}.";
}