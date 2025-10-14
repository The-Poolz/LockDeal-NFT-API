using FluentValidation;
using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public class LambdaRequestValidator : AbstractValidator<LambdaRequest>
{
    public static readonly string[] AllowedMethods = ["GET", "OPTIONS"];

    public LambdaRequestValidator()
    {
        RuleFor(request => request.PathParameters)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("Path parameters are required.")

            .Must(qsp => qsp.ContainsKey("chainId"))
            .WithMessage("Path parameter 'chainId' is required.")
            .Must(qsp => long.TryParse(qsp["chainId"], out _))
            .WithMessage("Path parameter 'chainId' must be a valid Int64.")

            .Must(qsp => qsp.ContainsKey("poolId"))
            .WithMessage("Path parameter 'poolId' is required.")
            .Must(qsp => long.TryParse(qsp["poolId"], out _))
            .WithMessage("Path parameter 'poolId' must be a valid Int64.");

        RuleFor(request => request.HttpMethod)
            .Must(httpMethod => AllowedMethods.Contains(httpMethod))
            .WithMessage(x => $"Allowed HTTP methods: ({string.Join(", ", AllowedMethods)}). Received HTTP method: {x.HttpMethod}");
    }
}