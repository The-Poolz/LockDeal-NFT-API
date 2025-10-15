namespace MetaDataAPI.Validation;

public static class LambdaRequestValidatorErrors
{
    public const string ExpectedPathPattern = "/{chainId}/{poolId}";

    public static string RawPathRequired() =>
        $"RawPath is required (expected format: '{ExpectedPathPattern}').";

    public static string RawPathWrongFormat(string? rawPath) =>
        $"RawPath must be '{ExpectedPathPattern}'. The first path parameter is 'chainId', the second is 'poolId'. Received: '{rawPath ?? string.Empty}'.";

    public static string ChainIdInvalid(string? rawPath) =>
        $"The first path parameter (chainId) must be a valid Int64. Received: '{GetSegment(rawPath, 0)}'.";

    public static string PoolIdInvalid(string? rawPath) =>
        $"The second path parameter (poolId) must be a valid Int64. Received: '{GetSegment(rawPath, 1)}'.";

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