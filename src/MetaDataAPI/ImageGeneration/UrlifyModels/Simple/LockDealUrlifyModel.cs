using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;

namespace MetaDataAPI.ImageGeneration.UrlifyModels.Simple;

public class LockDealUrlifyModel : BaseUrlifyModel
{
    [QueryStringProperty("Start Time", false)]
    public string StartTime { get; set; }

    public LockDealUrlifyModel(PoolInfo poolInfo) : base(poolInfo)
    {
        var dateTime = TimeUtils.FromUnixTimestamp((long)poolInfo.Params[1]);
        StartTime = $"{dateTime:MM/dd/yyyy} {dateTime:HH:mm:ss}";
    }
}