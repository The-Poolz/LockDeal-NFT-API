using FluentValidation;
using MetaDataAPI.Routing;
using MetaDataAPI.Routing.Requests;

namespace MetaDataAPI.Validation;

public class RouteApplicationLoadBalancerRequestValidator : AbstractValidator<RouteApplicationLoadBalancerRequest>
{
    public RouteApplicationLoadBalancerRequestValidator()
    {
        RuleFor(r => r.Request.HttpMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ValidatorErrorsMessages.HttpMethodRequired())
            .Must(BeAllowedMethod)
            .WithMessage(x => ValidatorErrorsMessages.HttpMethodNotAllowed(x.Request.HttpMethod, LambdaRoutes.AllowedMethods));

        When(IsNotOptionsRequest, () =>
        {
            RuleFor(r => r.Request.Path)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ValidatorErrorsMessages.PathRequired())
                .Must(BeAllowedPath)
                .WithMessage(x => ValidatorErrorsMessages.PathNotAllowed(x.Request.Path));
        });
    }

    private static bool IsNotOptionsRequest(RouteApplicationLoadBalancerRequest request) =>
        !string.Equals(request.Request.HttpMethod, LambdaRoutes.OptionsMethod, StringComparison.OrdinalIgnoreCase);

    private static bool BeAllowedPath(string? path) =>
        LambdaRoutes.IsMetadataPath(path) ||
        LambdaRoutes.IsFaviconPath(path) ||
        LambdaRoutes.IsHealthPath(path);

    private static bool BeAllowedMethod(string? method) =>
        LambdaRoutes.AllowedMethods.Contains(method ?? string.Empty, StringComparer.OrdinalIgnoreCase);
}