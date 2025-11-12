using System.Net.Http.Headers;
using Poolz.Finance.CSharp.Polly.Extensions;

namespace MetaDataAPI.Services.Http;

public class HttpClientFactory(IRetryExecutor retry) : IHttpClientFactory
{
    public HttpClient Create(string url, Action<HttpRequestHeaders>? configureHeaders = null)
    {
        var client = new HttpClient(new FailureOnlyLoggingHandler(new HttpClientHandler(), retry))
        {
            BaseAddress = new Uri(url)
        };

        configureHeaders?.Invoke(client.DefaultRequestHeaders);
        return client;
    }
}