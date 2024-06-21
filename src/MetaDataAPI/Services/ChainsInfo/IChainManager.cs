using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.ChainsInfo;

public interface IChainManager
{
    public bool TryFetchChainInfo(BigInteger chainId, [MaybeNullWhen(false)] out ChainInfo chainInfo);
}