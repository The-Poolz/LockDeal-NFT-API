using MetaDataAPI.Models.Response;
using MetaDataAPI.Utils;
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
        var converter = new ConvertWei(Erc20Token.Decimals);
        LeftAmount = converter.WeiToEth(poolInfo.Params[0]);
    }
}