using FluentValidation;
using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public sealed class LambdaRequestMetadataValidator : AbstractValidator<LambdaRequest>
{
    public LambdaRequestMetadataValidator()
    {
        RuleFor(r => r.Path)
            .Cascade(CascadeMode.Stop)
            .Must(HaveExactlyThreeSegments)
            .WithMessage(x => LambdaRequestValidatorErrors.PathWrongFormat(x.Path))
            .Must(BeValidChainId)
            .WithMessage(x => LambdaRequestValidatorErrors.ChainIdInvalid(x.Path))
            .Must(BeValidPoolId)
            .WithMessage(x => LambdaRequestValidatorErrors.PoolIdInvalid(x.Path));
    }

    private static bool HaveExactlyThreeSegments(string? path) =>
        LambdaRequestValidatorErrors.Split(path).Length == 3;

    private static bool BeValidChainId(string? path) =>
        long.TryParse(LambdaRequestValidatorErrors.GetSegment(path, 1), out _);

    private static bool BeValidPoolId(string? path) =>
        long.TryParse(LambdaRequestValidatorErrors.GetSegment(path, 2), out _);
}