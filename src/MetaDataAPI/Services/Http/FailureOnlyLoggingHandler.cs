using Amazon.Lambda.Core;
using Poolz.Finance.CSharp.Polly.Extensions;

namespace MetaDataAPI.Services.Http;

public class FailureOnlyLoggingHandler(HttpMessageHandler inner, IRetryExecutor retry) : DelegatingHandler(inner)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
    {
        try
        {
            var response = await retry.ExecuteAsync(
                async token => await base.SendAsync(req, token),
                new DefaultRetryStrategyOptions<HttpResponseMessage>(LambdaLogger.Log),
                ct
            );

            response.EnsureSuccessStatusCode();

            return response;
        }
        catch (HttpRequestException exception)
        {
            throw new HttpRequestException(
                $"HTTP request failed. METHOD: {req.Method}. URL: {req.RequestUri}",
                exception,
                exception.StatusCode
            );
        }
    }
}