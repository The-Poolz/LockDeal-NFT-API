using System.Numerics;

namespace MetaDataAPI.Services.ChainsInfo;

public class LocalChainManager : IChainManager
{
    private readonly IDictionary<BigInteger, ChainInfo> localChainInfo = new Dictionary<BigInteger, ChainInfo>
    {
        { 97, new ChainInfo(97, "https://data-seed-prebsc-1-s1.binance.org:8545/", "0xe42876a77108E8B3B2af53907f5e533Cba2Ce7BE") }
    };

    public ChainInfo FetchChainInfo(BigInteger chainId)
    {
        return localChainInfo[chainId];
    }
}