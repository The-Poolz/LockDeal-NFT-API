using System.Numerics;
using FluentValidation;
using MetaDataAPI.Request;

namespace MetaDataAPI.Validation;

public class LambdaRequestValidator : AbstractValidator<LambdaRequest>
{
    public LambdaRequestValidator()
    {
        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Query string parameters are required.");

        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .Must(qsp => qsp.ContainsKey("chainId"))
            .WithMessage("Query string parameter 'chainId' is required.")
            .Must(qsp => BigInteger.TryParse(qsp["chainId"], out _))
            .WithMessage("Query string parameter 'chainId' must be a valid BigInteger.");

        RuleFor(request => request.QueryStringParameters)
            .Cascade(CascadeMode.Stop)
            .Must(qsp => qsp.ContainsKey("poolId"))
            .WithMessage("Query string parameter 'poolId' is required.")
            .Must(qsp => BigInteger.TryParse(qsp["poolId"], out _))
            .WithMessage("Query string parameter 'poolId' must be a valid BigInteger.");
    }
}