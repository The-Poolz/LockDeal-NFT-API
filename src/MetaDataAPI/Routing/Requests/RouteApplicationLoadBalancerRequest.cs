using MediatR;
using MetaDataAPI.Models;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Routing.Requests;

public sealed record RouteApplicationLoadBalancerRequest(ApplicationLoadBalancerRequest Request) : IRequest<LambdaResponse>;