using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolsInfo;

public class CollateralPoolInfo : PoolInfo
{
    public decimal LeftAmount { get; }
    public uint FinishTimestamp { get; }
    public decimal Rate { get; }

    public CollateralPoolInfo(PoolInfo poolInfo, string rpcUrl) : base(poolInfo, rpcUrl)
    {
        LeftAmount = new ConvertWei(Token.Decimals).WeiToEth(Params[0]);
        FinishTimestamp = (uint)Params[1];
        Rate = new ConvertWei(21).WeiToEth(Params[2]);
    }
}