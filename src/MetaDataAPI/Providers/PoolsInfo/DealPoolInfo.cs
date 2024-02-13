using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolsInfo;

public class DealPoolInfo : PoolInfo
{
    public decimal LeftAmount { get; }

    public DealPoolInfo(PoolInfo poolInfo, string rpcUrl)
        : base(poolInfo, rpcUrl)
    {
        LeftAmount = new ConvertWei(Token.Decimals).WeiToEth(Params[0]);
    }
}