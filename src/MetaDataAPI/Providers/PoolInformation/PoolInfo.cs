using Nethereum.Util;
using Nethereum.Web3;
using MetaDataAPI.Erc20Manager;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using MetaDataAPI.Providers.Image.Models;

// ReSharper disable VirtualMemberCallInConstructor

namespace MetaDataAPI.Providers.PoolInformation;

public abstract class PoolInfo : BasePoolInfo
{
    public Erc20Token Erc20Token { get; }

    public BigDecimal LeftAmount { get; }

    public abstract string DescriptionTemplate { get; }
    public abstract dynamic DescriptionSource { get; }

    public abstract UrlifyModelCreation UrlifyModelCreation { get; }

    protected PoolInfo(BasePoolInfo poolInfo, Erc20Token erc20)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        Token = poolInfo.Token;
        Params = poolInfo.Params;

        Erc20Token = erc20;
        LeftAmount = Web3.Convert.FromWeiToBigDecimal(Params[0], Erc20Token.Decimals);
    }
}