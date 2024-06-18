using System.Numerics;
using Net.Web3.EthereumWallet;
using MetaDataAPI.BlockchainManager.Models;

namespace MetaDataAPI.Erc20Manager;

public interface IErc20Provider
{
    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address);
    public Erc20Token GetErc20Token(string rpcUrl, BigInteger chainId, EthereumAddress address);
}