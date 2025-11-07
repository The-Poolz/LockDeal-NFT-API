using Amazon.Lambda.Core;

namespace MetaDataAPI.Services.Logging;

public sealed class LambdaContextAccessor : ILambdaContextAccessor
{
    private static readonly AsyncLocal<ILambdaContext?> _ctx = new();
    public ILambdaContext? Context { get => _ctx.Value; set => _ctx.Value = value; }
}