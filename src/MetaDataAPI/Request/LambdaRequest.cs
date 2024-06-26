using System.Numerics;
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
            ChainId = validationResult.IsValid ? BigInteger.Parse(QueryStringParameters["chainId"]) : -1;
            PoolId = validationResult.IsValid ? BigInteger.Parse(QueryStringParameters["poolId"]) : -1;
            return validationResult.IsValid ? null : validationResult;
        }
    }

    public BigInteger PoolId { get; private set; } = -1;
    public BigInteger ChainId { get; private set; } = -1;
}