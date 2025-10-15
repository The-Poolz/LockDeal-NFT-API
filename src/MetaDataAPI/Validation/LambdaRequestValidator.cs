using FluentValidation;
using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public class LambdaRequestValidator : AbstractValidator<LambdaRequest>
{
    public static readonly string[] AllowedMethods = ["GET", "OPTIONS"];

    public LambdaRequestValidator()
    {
        RuleFor(r => r.RawPath)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(LambdaRequestValidatorErrors.RawPathRequired())
            .Must(HaveExactlyTwoSegments)
            .WithMessage(x => LambdaRequestValidatorErrors.RawPathWrongFormat(x.RawPath))
            .Must(BeValidChainId)
            .WithMessage(x => LambdaRequestValidatorErrors.ChainIdInvalid(x.RawPath))
            .Must(BeValidPoolId)
            .WithMessage(x => LambdaRequestValidatorErrors.PoolIdInvalid(x.RawPath));

        RuleFor(x => x.HttpMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(LambdaRequestValidatorErrors.HttpMethodRequired())
            .Must(m => AllowedMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
            .WithMessage(x => LambdaRequestValidatorErrors.HttpMethodNotAllowed(x.HttpMethod, AllowedMethods));
    }

    private static bool HaveExactlyTwoSegments(string? rawPath) => LambdaRequestValidatorErrors.Split(rawPath).Length == 2;
    private static bool BeValidChainId(string? rawPath) => long.TryParse(LambdaRequestValidatorErrors.GetSegment(rawPath, 0), out _);
    private static bool BeValidPoolId(string? rawPath) => long.TryParse(LambdaRequestValidatorErrors.GetSegment(rawPath, 1), out _);
}