using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Logging;

public sealed class StrictLambdaLogger(ILambdaContextAccessor accessor) : ILambdaLogger
{
    private ILambdaLogger Current =>
        accessor.Context?.Logger
        ?? throw new InvalidOperationException(
            "ILambdaLogger is unavailable because ILambdaContextAccessor.Context is null. " +
            "Set ILambdaContextAccessor.Context in FunctionHandler before resolving services.");

    public void Log(string message) => Current.Log(message);
    public void LogLine(string message) => Current.LogLine(message);
}