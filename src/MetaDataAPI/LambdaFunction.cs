using MediatR;
using FluentValidation;
using Amazon.Lambda.Core;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Errors;
using MetaDataAPI.Routing.Requests;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction(IServiceProvider root)
{
    public LambdaFunction() : this(DefaultServiceProvider.Instance) { }

    public async Task<LambdaResponse> FunctionHandler(ApplicationLoadBalancerRequest request, ILambdaContext lambdaContext)
    {
        using var scope = root.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        try
        {
            return await mediator.Send(new RouteApplicationLoadBalancerRequest(request));
        }
        catch (ValidationException ex)
        {
            return new ValidationErrorResponse(ex);
        }
        catch (Exception ex)
        {
            lambdaContext.Logger.LogError(ex.ToString());
            return new GeneralErrorResponse();
        }
    }
}
