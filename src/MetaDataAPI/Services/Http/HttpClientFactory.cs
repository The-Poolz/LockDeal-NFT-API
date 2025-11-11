using System.Net.Http.Headers;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Services.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public const int RequestTimeoutSeconds = 5;

    public HttpClient Create(string url, Action<HttpRequestHeaders>? configureHeaders = null)
    {
        var client = new HttpClient(new FailureOnlyLoggingHandler(new HttpClientHandler()))
        {
            BaseAddress = new Uri(url),
            Timeout = TimeSpan.FromSeconds(Env.HTTP_CALL_TIMEOUT_IN_SECONDS.GetOrDefault(RequestTimeoutSeconds))
        };

        configureHeaders?.Invoke(client.DefaultRequestHeaders);
        return client;
    }
}