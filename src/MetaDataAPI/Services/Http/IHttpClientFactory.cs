using System.Net.Http.Headers;

namespace MetaDataAPI.Services.Http;

public interface IHttpClientFactory
{
    public HttpClient Create(string url, Action<HttpRequestHeaders>? configure = null);
}