using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.ImageGeneration.UrlifyModels.Simple;

public class TimedDealUrlifyModel : LockDealUrlifyModel
{
    [QueryStringProperty("Finish Time", false)]
    public string FinishTime { get; set; }

    public TimedDealUrlifyModel(BasePoolInfo poolInfo) : base(poolInfo)
    {
        var dateTime = TimeUtils.FromUnixTimestamp((long)poolInfo.Params[2]);
        FinishTime = $"{dateTime:MM/dd/yyyy} {dateTime:HH:mm:ss}";
    }
}