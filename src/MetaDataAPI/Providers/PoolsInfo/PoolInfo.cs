using System.Numerics;
using MetaDataAPI.RPC;
using MetaDataAPI.RPC.Models;
using MetaDataAPI.RPC.Models.Functions.Outputs;
using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Providers.PoolsInfo;

public class PoolInfo
{
    public EthereumAddress Provider { get; }
    public string Name { get; }
    public BigInteger PoolId { get; }
    public BigInteger VaultId { get; }
    public EthereumAddress Owner { get; }
    public EthereumAddress TokenAddress { get; }
    public List<BigInteger> Params { get; }
    public ERC20Token Token { get; }

    public PoolInfo(
        EthereumAddress provider,
        string name,
        BigInteger poolId,
        BigInteger vaultId,
        EthereumAddress owner,
        EthereumAddress tokenAddress,
        List<BigInteger> parameters,
        ERC20Token token
    )
    {
        Provider = provider;
        Name = name;
        PoolId = poolId;
        VaultId = vaultId;
        Owner = owner;
        TokenAddress = tokenAddress;
        Params = parameters;
        Token = token;
    }

    public PoolInfo(PoolInfo poolInfo, string rpcUrl)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        TokenAddress = poolInfo.TokenAddress;
        Params = poolInfo.Params;
        Token = new ERC20Token(poolInfo.TokenAddress, new ERC20Service(rpcUrl, poolInfo.TokenAddress));
    }

    public PoolInfo(BasePoolInfo poolInfo, string rpcUrl)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        TokenAddress = poolInfo.Token;
        Params = poolInfo.Params;
        Token = new ERC20Token(poolInfo.Token, new ERC20Service(rpcUrl, poolInfo.Token));
    }
}