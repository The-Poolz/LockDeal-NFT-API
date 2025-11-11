namespace MetaDataAPI.Services.Http;

public class FailureOnlyLoggingHandler(HttpMessageHandler inner) : DelegatingHandler(inner)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
    {
        try
        {
            var response = await base.SendAsync(req, ct);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException exception)
        {
            throw new HttpRequestException(
                $"HTTP EXCEPTION OCCURED. METHOD: {req.Method}. URL: {req.RequestUri}",
                exception,
                exception.StatusCode
            );
        }
    }
}