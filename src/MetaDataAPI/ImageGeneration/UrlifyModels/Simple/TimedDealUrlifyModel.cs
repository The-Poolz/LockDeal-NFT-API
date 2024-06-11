using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;

namespace MetaDataAPI.ImageGeneration.UrlifyModels.Simple;

public class TimedDealUrlifyModel : LockDealUrlifyModel
{
    [QueryStringProperty("Finish Time", false)]
    public string FinishTime { get; set; }

    public TimedDealUrlifyModel(PoolInfo poolInfo) : base(poolInfo)
    {
        var dateTime = TimeUtils.FromUnixTimestamp((long)poolInfo.Params[2]);
        FinishTime = $"{dateTime:MM/dd/yyyy} {dateTime:HH:mm:ss}";
    }
}