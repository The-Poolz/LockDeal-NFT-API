using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;

namespace MetaDataAPI.ImageGeneration.UrlifyModels;

public class TimedDealUrlifyModel : LockDealUrlifyModel
{
    [QueryStringProperty("Finish Time")]
    public string FinishTime { get; set; }

    public TimedDealUrlifyModel(PoolInfo poolInfo) : base(poolInfo)
    {
        FinishTime = $"{TimeUtils.FromUnixTimestamp((long)poolInfo.Params[2]):g}";
    }
}