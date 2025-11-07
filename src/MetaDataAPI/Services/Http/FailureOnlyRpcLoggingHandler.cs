using System.Text;
using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Http;

public class FailureOnlyRpcLoggingHandler(HttpMessageHandler inner, ILambdaLogger log) : DelegatingHandler(inner)
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
                .AppendLine($"RPC HTTP EXCEPTION {req.Method} {req.RequestUri}")
                .AppendLine($"EXCEPTION: {ex}")
                .ToString();
            log.LogCritical(logMessage);
            throw;
        }
    }
}