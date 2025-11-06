using MediatR;
using MetaDataAPI.Models;

namespace MetaDataAPI.Routing.Requests;

public sealed record GetMetadataRequest(string Path) : IRequest<LambdaResponse>;