using Newtonsoft.Json;
using MetaDataAPI.Validation;
using FluentValidation.Results;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Models;

public class LambdaRequest : APIGatewayHttpApiV2ProxyRequest
{
    [JsonConstructor]
    public LambdaRequest(ProxyRequestContext requestContext, string rawPath)
    {
        RawPath = rawPath;
        HttpMethod = requestContext.Http?.Method?.ToUpperInvariant() ?? string.Empty;

        ValidationResult = new LambdaRequestValidator().Validate(this);

        if (ValidationResult.IsValid)
        {
            var parts = rawPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            ChainId = long.Parse(parts[0]);
            PoolId = long.Parse(parts[1]);
        }
        else
        {
            ChainId = 0;
            PoolId = 0;
        }
    }

    public ValidationResult ValidationResult { get; }
    public string HttpMethod { get; }
    public long PoolId { get; }
    public long ChainId { get; }
}