using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Logging;

public interface ILambdaContextAccessor
{
    ILambdaContext? Context { get; set; }
}