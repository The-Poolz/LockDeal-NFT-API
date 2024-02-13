using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolsInfo;

public class RefundPoolInfo : PoolInfo
{
    public decimal TokenLeftAmount { get; }

    public RefundPoolInfo(PoolInfo poolInfo, string rpcUrl) : base(poolInfo, rpcUrl)
    {
        TokenLeftAmount = new ConvertWei(poolInfo.Token.Decimals).WeiToEth(Params[0]);
    }
}