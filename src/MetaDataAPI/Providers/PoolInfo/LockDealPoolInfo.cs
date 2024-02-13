using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolInfo;

public class LockDealPoolInfo : DealPoolInfo
{
    public uint StartTime { get; }
    public DateTime StartDateTime { get; }

    public LockDealPoolInfo(BasePoolInfo poolInfo, string rpcUrl) : base(poolInfo, rpcUrl)
    {
        StartTime = (uint)Params[1];
        StartDateTime = TimeUtils.FromUnixTimestamp(StartTime);
    }
}