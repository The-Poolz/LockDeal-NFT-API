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
            .Must(BeAllowedPath)
            .WithMessage(x => LambdaRequestValidatorErrors.PathNotAllowed(x.Path));

        When(IsMetadataRequest, () =>
        {
            Include(new LambdaRequestMetadataValidator());
        });

        RuleFor(x => x.HttpMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(LambdaRequestValidatorErrors.HttpMethodRequired())
            .Must(m => LambdaRequest.AllowedMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
            .WithMessage(x => LambdaRequestValidatorErrors.HttpMethodNotAllowed(x.HttpMethod, LambdaRequest.AllowedMethods));
    }

    private static bool BeAllowedPath(string? path) => LambdaRequest.IsMetadataPath(path) || LambdaRequest.IsFaviconPath(path);

    private static bool IsMetadataRequest(LambdaRequest request) => LambdaRequest.IsMetadataPath(request.Path);
}