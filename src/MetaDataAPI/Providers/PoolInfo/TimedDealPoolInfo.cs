using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolInfo;

public class TimedDealPoolInfo : LockDealPoolInfo
{
    public uint FinishTime { get; }
    public DateTime FinishDateTime { get; }
    public decimal StartAmount { get; }

    public TimedDealPoolInfo(BasePoolInfo poolInfo, string rpcUrl) : base(poolInfo, rpcUrl)
    {
        FinishTime = (uint)Params[2];
        FinishDateTime = TimeUtils.FromUnixTimestamp(FinishTime);
        StartAmount = new ConvertWei(Token.Decimals).WeiToEth(Params[3]);
    }
}