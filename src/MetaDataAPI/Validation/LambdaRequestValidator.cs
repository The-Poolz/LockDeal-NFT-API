using FluentValidation;
using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public class LambdaRequestValidator : AbstractValidator<LambdaRequest>
{
    public LambdaRequestValidator()
    {
        RuleFor(r => r.Path)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(LambdaRequestValidatorErrors.PathRequired())
            .Must(HaveExactlyThreeSegments)
            .WithMessage(x => LambdaRequestValidatorErrors.PathWrongFormat(x.Path))
            .Must(BeValidChainId)
            .WithMessage(x => LambdaRequestValidatorErrors.ChainIdInvalid(x.Path))
            .Must(BeValidPoolId)
            .WithMessage(x => LambdaRequestValidatorErrors.PoolIdInvalid(x.Path));

        RuleFor(x => x.HttpMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(LambdaRequestValidatorErrors.HttpMethodRequired())
            .Must(m => LambdaRequest.AllowedMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
            .WithMessage(x => LambdaRequestValidatorErrors.HttpMethodNotAllowed(x.HttpMethod, LambdaRequest.AllowedMethods));
    }

    private static bool HaveExactlyThreeSegments(string? path) => LambdaRequestValidatorErrors.Split(path).Length == 3;
    private static bool BeValidChainId(string? path) => long.TryParse(LambdaRequestValidatorErrors.GetSegment(path, 1), out _);
    private static bool BeValidPoolId(string? path) => long.TryParse(LambdaRequestValidatorErrors.GetSegment(path, 2), out _);
}