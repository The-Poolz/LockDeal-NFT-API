using System.Numerics;
using MetaDataAPI.Validation;
using FluentValidation.Results;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaDataAPI.Request;

public class LambdaRequest : APIGatewayProxyRequest
{
    public ValidationResult? ValidationResult { get; }

    public LambdaRequest(APIGatewayProxyRequest request)
    {
        var validationResult = new LambdaRequestValidator().Validate(this);
        ValidationResult = validationResult.IsValid ? null : validationResult;
        ChainId = validationResult.IsValid ? BigInteger.Parse(request.QueryStringParameters["chainId"]) : -1;
        PoolId = validationResult.IsValid ? BigInteger.Parse(request.QueryStringParameters["poolId"]) : -1;
    }

    public BigInteger ChainId { get; }
    public BigInteger PoolId { get; }
}