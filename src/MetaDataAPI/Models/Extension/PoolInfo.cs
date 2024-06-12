using MetaDataAPI.Extensions;
using MetaDataAPI.Models.Response;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Models.Extension;

public sealed class PoolInfo : BasePoolInfo
{
    public Erc20Token Erc20Token { get; }
    public decimal LeftAmount { get; }

    public PoolInfo(BasePoolInfo poolInfo)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        Token = poolInfo.Token;
        Params = poolInfo.Params;

        Erc20Token = new Erc20Token(poolInfo.Token);
        LeftAmount = poolInfo.Params[0].WeiToEth(Erc20Token.Decimals);
    }
}