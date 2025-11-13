using System.Net.Http.Headers;

namespace MetaDataAPI.Services.Http;

public class HttpClientFactory(System.Net.Http.IHttpClientFactory innerFactory) : IHttpClientFactory
{
    public const string GenericClientName = "GenericClient";

    public HttpClient Create(string url, Action<HttpRequestHeaders>? configureHeaders = null)
    {
        var client = innerFactory.CreateClient(GenericClientName);
        client.BaseAddress = new Uri(url);

        configureHeaders?.Invoke(client.DefaultRequestHeaders);

        return client;
    }
}