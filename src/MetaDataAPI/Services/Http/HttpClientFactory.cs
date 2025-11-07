using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient Create(string url, ILambdaLogger log)
    {
        return new HttpClient(new FailureOnlyLoggingHandler(new HttpClientHandler(), log))
        {
            BaseAddress = new Uri(url),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }
}