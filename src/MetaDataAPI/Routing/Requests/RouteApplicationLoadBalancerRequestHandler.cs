using MediatR;
using FluentValidation;
using MetaDataAPI.Models;
using MetaDataAPI.Validation;

namespace MetaDataAPI.Routing.Requests;

public class RouteApplicationLoadBalancerRequestHandler(IMediator mediator) : IRequestHandler<RouteApplicationLoadBalancerRequest, LambdaResponse>
{
    public async Task<LambdaResponse> Handle(RouteApplicationLoadBalancerRequest request, CancellationToken cancellationToken)
    {
        var albRequest = request.Request;
        var httpMethod = albRequest.HttpMethod ?? string.Empty;
        var path = albRequest.Path ?? string.Empty;

        if (string.Equals(httpMethod, LambdaRoutes.OptionsMethod, StringComparison.OrdinalIgnoreCase))
        {
            return new OptionsResponse();
        }

        if (LambdaRoutes.IsFaviconPath(path))
        {
            return new FaviconLambdaResponse();
        }

        if (LambdaRoutes.IsMetadataPath(path))
        {
            return await mediator.Send(new GetMetadataRequest(path), cancellationToken);
        }

        throw new ValidationException(ValidatorErrorsMessages.PathNotAllowed(path));
    }
}