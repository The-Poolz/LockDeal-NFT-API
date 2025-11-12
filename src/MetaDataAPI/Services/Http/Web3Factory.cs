using Nethereum.Web3;
using Nethereum.JsonRpc.Client;

namespace MetaDataAPI.Services.Http;

public class Web3Factory(IHttpClientFactory httpClientFactory) : IWeb3Factory
{
    public IWeb3 Create(string rpcUrl)
    {
        var httpClient = httpClientFactory.Create(rpcUrl);
        var rpcClient = new RpcClient(new Uri(rpcUrl), httpClient);
        return new Web3(rpcClient);
    }
}