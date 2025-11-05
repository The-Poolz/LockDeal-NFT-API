using FluentValidation;
using MetaDataAPI.Models;
using MetaDataAPI.Extensions;

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
        path.SplitPath().Length == 3;

    private static bool BeValidChainId(string? path) =>
        long.TryParse(path.GetSegment(1), out _);

    private static bool BeValidPoolId(string? path) =>
        long.TryParse(path.GetSegment(2), out _);
}