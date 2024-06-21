using System.Numerics;
using FluentValidation;
using MetaDataAPI.Request;

namespace MetaDataAPI.Validation;

public class APIGatewayProxyRequestValidator : AbstractValidator<LambdaRequest>
{
    public APIGatewayProxyRequestValidator()
    {
        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Query string parameters are required.");

        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .Must(qsp => qsp.ContainsKey("chainId") && BigInteger.TryParse(qsp["chainId"], out _))
            .WithMessage("Query string parameter 'chainId' is required and must be a valid BigInteger.")
            .DependentRules(() =>
                RuleFor(request => request.ChainId)
                    .NotNull()
                    .DependentRules(() =>
                        RuleFor(request => request.ChainId!.Value)
                            .GreaterThanOrEqualTo(1).WithMessage("chainId must be non-negative.")
                    )
            );

        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .Must(qsp => qsp.ContainsKey("poolId") && BigInteger.TryParse(qsp["poolId"], out _))
            .WithMessage("Query string parameter 'poolId' is required and must be a valid BigInteger.")
            .DependentRules(() =>
                RuleFor(request => request.PoolId)
                    .NotNull()
                    .DependentRules(() =>
                        RuleFor(request => request.PoolId!.Value)
                            .GreaterThanOrEqualTo(0).WithMessage("poolId must be non-negative.")
                    )
            );
    }
}