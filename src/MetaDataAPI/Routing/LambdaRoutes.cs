using MetaDataAPI.Extensions;

namespace MetaDataAPI.Routing;

public static class LambdaRoutes
{
    public const string GetMethod = "GET";
    public const string OptionsMethod = "OPTIONS";
    public const string MetadataSegmentName = "metadata";
    public const string FaviconPath = "/favicon.ico";
    public const string HealthPath = "/health";

    public static readonly string[] AllowedMethods = [GetMethod, OptionsMethod];

    public static bool IsMetadataPath(string? path) =>
        string.Equals(path.GetSegment(0), MetadataSegmentName, StringComparison.OrdinalIgnoreCase);

    public static bool IsFaviconPath(string? path) =>
        string.Equals(path, FaviconPath, StringComparison.OrdinalIgnoreCase);

    public static bool IsHealthPath(string? path) =>
        string.Equals(path, HealthPath, StringComparison.OrdinalIgnoreCase);
}