using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.ImageGeneration.UrlifyModels.Simple;

public class LockDealUrlifyModel : DealUrlifyModel
{
    [QueryStringProperty("Start Time", false)]
    public string StartTime { get; set; }

    public LockDealUrlifyModel(BasePoolInfo poolInfo) : base(poolInfo)
    {
        var dateTime = TimeUtils.FromUnixTimestamp((long)poolInfo.Params[1]);
        StartTime = $"{dateTime:MM/dd/yyyy} {dateTime:HH:mm:ss}";
    }
}