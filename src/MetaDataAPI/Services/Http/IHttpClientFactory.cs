namespace MetaDataAPI.Services.Http;

public interface IHttpClientFactory
{
    public HttpClient Create(string url);
}