using System.Numerics;
using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Erc20Manager;

public interface IErc20Provider
{
    public Erc20Token GetErc20Token(string rpcUrl, BigInteger chainId, EthereumAddress address);
}