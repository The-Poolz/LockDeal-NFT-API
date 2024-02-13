using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.PoolInfo;

public class DealPoolInfo : BasePoolInfo
{
    public decimal LeftAmount { get; }

    public DealPoolInfo(BasePoolInfo poolInfo, string rpcUrl)
        : base(poolInfo, rpcUrl)
    {
        LeftAmount = new ConvertWei(Token.Decimals).WeiToEth(Params[0]);
    }
}