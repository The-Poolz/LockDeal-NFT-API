using System.Numerics;
using Net.Web3.EthereumWallet;
using MetaDataAPI.RPC.Models.DTOs;

namespace MetaDataAPI.RPC.Models.PoolInfo;

public class BasePoolInfo
{
    public EthereumAddress Provider { get; }
    public string Name { get; }
    public BigInteger PoolId { get; }
    public BigInteger VaultId { get; }
    public EthereumAddress Owner { get; }
    public EthereumAddress Token { get; }
    public List<BigInteger> Params { get; }

    public BasePoolInfo(EthereumAddress provider, string name, BigInteger poolId, BigInteger vaultId, EthereumAddress owner, EthereumAddress token, List<BigInteger> parameters)
    {
        Provider = provider;
        Name = name;
        PoolId = poolId;
        VaultId = vaultId;
        Owner = owner;
        Token = token;
        Params = parameters;
    }

    public BasePoolInfo(BasePoolInfoDTO poolInfo)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        Token = poolInfo.Token;
        Params = poolInfo.Params;
    }

    public virtual ERC20Token GetERC20Token(EthereumAddress tokenAddress, string rpcUrl) => new(tokenAddress, new ERC20Service(rpcUrl, tokenAddress));
    public virtual ERC20Token GetERC20Token(EthereumAddress tokenAddress, ERC20Service erc20Service) => new(tokenAddress, erc20Service);
}