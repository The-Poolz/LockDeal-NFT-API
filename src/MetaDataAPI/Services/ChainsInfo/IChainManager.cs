using System.Numerics;

namespace MetaDataAPI.Services.ChainsInfo;

public interface IChainManager
{
    public ChainInfo FetchChainInfo(BigInteger chainId);
}