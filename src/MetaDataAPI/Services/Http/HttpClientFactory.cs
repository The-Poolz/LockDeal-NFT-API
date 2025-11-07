using Amazon.Lambda.Core;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Services.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient Create(string url, ILambdaLogger log)
    {
        return new HttpClient(new FailureOnlyLoggingHandler(new HttpClientHandler(), log))
        {
            BaseAddress = new Uri(url),
            Timeout = TimeSpan.FromSeconds(Env.HTTP_CALL_TIMEOUT_IN_SECONDS.GetRequired<int>())
        };
    }
}