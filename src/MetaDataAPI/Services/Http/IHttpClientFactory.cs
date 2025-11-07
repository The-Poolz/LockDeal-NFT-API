using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public interface IHttpClientFactory
{
    public HttpClient Create(string url, ILambdaLogger log);
}