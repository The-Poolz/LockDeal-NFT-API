using Newtonsoft.Json;
using MetaDataAPI.Validation;
using FluentValidation.Results;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Models;

public class LambdaRequest : APIGatewayHttpApiV2ProxyRequest
{
    [JsonConstructor]
    public LambdaRequest(ProxyRequestContext context, IDictionary<string, string> pathParameters)
    {
        PathParameters = pathParameters;
        HttpMethod = context.Http.Method;
        ValidationResult = new LambdaRequestValidator().Validate(this);
        ChainId = ValidationResult.IsValid ? long.Parse(PathParameters["chainId"]) : 0;
        PoolId = ValidationResult.IsValid ? long.Parse(PathParameters["poolId"]) : 0;
    }

    public ValidationResult ValidationResult { get; }
    public string HttpMethod { get; }
    public long PoolId { get; }
    public long ChainId { get; }
}