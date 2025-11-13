using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public class FailureOnlyLoggingHandler : DelegatingHandler
{
    public FailureOnlyLoggingHandler() { }

    public FailureOnlyLoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

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
            LambdaLogger.Log(
                $"HTTP request failed. METHOD: {req.Method}. URL: {req.RequestUri}. STATUS: {exception.StatusCode}"
            );

            throw new HttpRequestException(
                $"HTTP request failed. METHOD: {req.Method}. URL: {req.RequestUri}",
                exception,
                exception.StatusCode
            );
        }
    }
}