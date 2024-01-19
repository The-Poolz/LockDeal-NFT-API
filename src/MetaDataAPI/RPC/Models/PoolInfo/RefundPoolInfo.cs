using MetaDataAPI.Utils;

namespace MetaDataAPI.RPC.Models.PoolInfo;

public class RefundPoolInfo : BasePoolInfo
{
    public decimal TokenLeftAmount { get; }

    public RefundPoolInfo(BasePoolInfo poolInfo, string rpcUrl) : base(poolInfo, rpcUrl)
    {
        TokenLeftAmount = new ConvertWei(poolInfo.Token.Decimals).WeiToEth(Params[0]);
    }
}