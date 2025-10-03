using MetaDataAPI.Validation;
using FluentValidation.Results;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Request;

public class LambdaRequest : APIGatewayProxyRequest
{
    public ValidationResult? ValidationResult
    {
        get {
            var validationResult = new LambdaRequestValidator().Validate(this);
            ChainId = validationResult.IsValid ? long.Parse(QueryStringParameters["chainId"]) : 0;
            PoolId = validationResult.IsValid ? long.Parse(QueryStringParameters["poolId"]) : 0;
            return validationResult.IsValid ? null : validationResult;
        }
    }

    public long PoolId { get; private set; }
    public long ChainId { get; private set; }
}