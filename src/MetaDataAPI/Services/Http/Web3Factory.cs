using Nethereum.Web3;
using Amazon.Lambda.Core;
using Nethereum.JsonRpc.Client;

namespace MetaDataAPI.Services.Http;

public class Web3Factory(IHttpClientFactory httpClientFactory) : IWeb3Factory
{
    public IWeb3 Create(string rpcUrl, ILambdaLogger log)
    {
        var httpClient = httpClientFactory.Create(rpcUrl, log);
        var rpcClient = new RpcClient(new Uri(rpcUrl), httpClient);
        return new Web3(rpcClient);
    }
}