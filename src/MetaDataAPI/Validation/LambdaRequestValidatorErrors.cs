using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public static class LambdaRequestValidatorErrors
{
    public const string ExpectedPathPattern = "/metadata/{chainId}/{poolId}";
    private static readonly string[] AllowedPathsForMessage = [ExpectedPathPattern, LambdaRequest.FaviconPath];

    public static string PathRequired() =>
        $"Path is required (expected format: '{ExpectedPathPattern}').";

    public static string PathWrongFormat(string? rawPath) =>
        $"Path must be '{ExpectedPathPattern}'. The first path parameter is 'metadata', the second is 'chainId', the third is 'poolId'. Received: '{rawPath ?? string.Empty}'.";

    public static string PathNotAllowed(string? rawPath) =>
        $"Path is not allowed. Allowed paths: {string.Join(", ", AllowedPathsForMessage.Select(p => $"'{p}'"))}. Received: '{rawPath ?? string.Empty}'.";

    public static string ChainIdInvalid(string? rawPath) =>
        $"The second path parameter (chainId) must be a valid Int64. Received: '{GetSegment(rawPath, 1)}'.";

    public static string PoolIdInvalid(string? rawPath) =>
        $"The third path parameter (poolId) must be a valid Int64. Received: '{GetSegment(rawPath, 2)}'.";

    public static string HttpMethodRequired() => "HTTP method is required.";

    public static string HttpMethodNotAllowed(string? method, IEnumerable<string> allowed) =>
        $"Allowed HTTP methods: ({string.Join(", ", allowed)}). Received HTTP method: {method}";

    public static string[] Split(string? rawPath) =>
        (rawPath ?? string.Empty).Split('/', StringSplitOptions.RemoveEmptyEntries);

    public static string GetSegment(string? rawPath, int index)
    {
        var parts = Split(rawPath);
        return parts.Length > index ? parts[index] : string.Empty;
    }
}