using Nethereum.Web3;

namespace MetaDataAPI.Services.Http;

public interface IWeb3Factory
{
    public IWeb3 Create(string rpcUrl);
}