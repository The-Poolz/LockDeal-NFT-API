using FluentValidation;
using MetaDataAPI.Extensions;
using MetaDataAPI.Routing.Requests;

namespace MetaDataAPI.Validation;

public sealed class GetMetadataRequestValidator : AbstractValidator<GetMetadataRequest>
{
    public GetMetadataRequestValidator()
    {
        RuleFor(r => r.Path)
            .Cascade(CascadeMode.Stop)
            .Must(HaveExactlyThreeSegments)
            .WithMessage(x => ValidatorErrorsMessages.PathWrongFormat(x.Path))
            .Must(BeValidChainId)
            .WithMessage(x => ValidatorErrorsMessages.ChainIdInvalid(x.Path))
            .Must(BeValidPoolId)
            .WithMessage(x => ValidatorErrorsMessages.PoolIdInvalid(x.Path));
    }

    private static bool HaveExactlyThreeSegments(string? path) =>
        path.SplitPath().Length == 3;

    private static bool BeValidChainId(string? path) =>
        long.TryParse(path.GetSegment(1), out _);

    private static bool BeValidPoolId(string? path) =>
        long.TryParse(path.GetSegment(2), out _);
}