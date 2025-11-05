namespace MetaDataAPI.Extensions;

public static class StringExtensions
{
    public static string GetSegment(this string? rawPath, int index)
    {
        var parts = SplitPath(rawPath);
        return parts.Length > index ? parts[index] : string.Empty;
    }

    public static string[] SplitPath(this string? rawPath) =>
        (rawPath ?? string.Empty).Split('/', StringSplitOptions.RemoveEmptyEntries);
}