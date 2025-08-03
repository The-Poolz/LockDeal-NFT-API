using MetaDataAPI.Services.ChainsInfo;

namespace MetaDataAPI.Services.Strapi;

public interface IStrapiClient
{
    public Task<ChainInfo?> GetChainInfoAsync(long chainId);
}