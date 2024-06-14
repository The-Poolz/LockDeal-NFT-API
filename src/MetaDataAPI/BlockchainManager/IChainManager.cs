using System.Numerics;
using MetaDataAPI.BlockchainManager.Models;

namespace MetaDataAPI.BlockchainManager;

public interface IChainManager
{
    public ChainInfo FetchChainInfo(BigInteger chainId);
}