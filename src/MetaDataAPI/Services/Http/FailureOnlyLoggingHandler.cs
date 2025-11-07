using System.Text;
using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public class FailureOnlyLoggingHandler(HttpMessageHandler inner, ILambdaLogger log) : DelegatingHandler(inner)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
    {
        try
        {
            return await base.SendAsync(req, ct);
        }
        catch (HttpRequestException ex)
        {
            var logMessage = new StringBuilder()
                .AppendLine($"HTTP EXCEPTION OCCURED. METHOD: {req.Method}. URL: {req.RequestUri}")
                .AppendLine($"EXCEPTION: {ex}")
                .ToString();
            log.LogCritical(logMessage);
            throw;
        }
    }
}