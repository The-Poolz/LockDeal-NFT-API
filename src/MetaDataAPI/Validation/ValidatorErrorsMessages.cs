using MetaDataAPI.Routing;
using MetaDataAPI.Extensions;

namespace MetaDataAPI.Validation;

public static class ValidatorErrorsMessages
{
    public const string ExpectedPathPattern = "/metadata/{chainId}/{poolId}";
    private static readonly string[] AllowedPathsForMessage =
    [
        ExpectedPathPattern,
        LambdaRoutes.FaviconPath,
        LambdaRoutes.HealthPath
    ];

    public static string PathRequired() =>
        $"Path is required (expected format: '{ExpectedPathPattern}').";

    public static string PathWrongFormat(string? rawPath) =>
        $"Path must be '{ExpectedPathPattern}'. The first path parameter is 'metadata', the second is 'chainId', the third is 'poolId'. Received: '{rawPath ?? string.Empty}'.";

    public static string PathNotAllowed(string? rawPath) =>
        $"Path is not allowed. Allowed paths: {string.Join(", ", AllowedPathsForMessage.Select(p => $"'{p}'"))}. Received: '{rawPath ?? string.Empty}'.";

    public static string ChainIdInvalid(string? rawPath) =>
        $"The second path parameter (chainId) must be a valid Int64. Received: '{rawPath.GetSegment(1)}'.";

    public static string PoolIdInvalid(string? rawPath) =>
        $"The third path parameter (poolId) must be a valid Int64. Received: '{rawPath.GetSegment(2)}'.";

    public static string HttpMethodRequired() => "HTTP method is required.";

    public static string HttpMethodNotAllowed(string? method, IEnumerable<string> allowed) =>
        $"Allowed HTTP methods: ({string.Join(", ", allowed)}). Received HTTP method: {method}";
}