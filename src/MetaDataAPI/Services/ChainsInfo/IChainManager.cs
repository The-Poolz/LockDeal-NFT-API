using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.ChainsInfo;

public interface IChainManager
{
    public bool TryFetchChainInfo(long chainId, [MaybeNullWhen(false)] out ChainInfo chainInfo);
}