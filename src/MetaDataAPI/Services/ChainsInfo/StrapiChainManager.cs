using MetaDataAPI.Services.Strapi;
using System.Diagnostics.CodeAnalysis;

namespace MetaDataAPI.Services.ChainsInfo;

public class StrapiChainManager(IStrapiClient client) : IChainManager
{
    public bool TryFetchChainInfo(long chainId, [MaybeNullWhen(false)] out ChainInfo chainInfo)
    {
        chainInfo = client.GetChainInfoAsync(chainId)
            .GetAwaiter()
            .GetResult();

        return chainInfo != null;
    }
}