using Nethereum.Web3;
using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public interface IWeb3Factory
{
    public IWeb3 Create(string rpcUrl, ILambdaLogger log);
}