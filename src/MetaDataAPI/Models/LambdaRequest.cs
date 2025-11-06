using Newtonsoft.Json;
using MetaDataAPI.Validation;
using MetaDataAPI.Extensions;
using FluentValidation.Results;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Models;

public class LambdaRequest : ApplicationLoadBalancerRequest
{
    public const string GET_METHOD = "GET";
    public const string OPTIONS_METHOD = "OPTIONS";
    public const string MetadataSegmentName = "metadata";
    public const string FaviconPath = "/favicon.ico";
    public static readonly string[] AllowedMethods = [GET_METHOD, OPTIONS_METHOD];

    [JsonConstructor]
    public LambdaRequest(string httpMethod, string path)
    {
        Path = path;
        HttpMethod = httpMethod;

        IsFaviconRequest = IsFaviconPath(path);
        IsMetadataRequest = IsMetadataPath(path);

        ValidationResult = new LambdaRequestValidator().Validate(this);

        if (ValidationResult.IsValid && IsMetadataRequest)
        {
            var parts = path.SplitPath();
            ChainId = long.Parse(parts[1]);
            PoolId = long.Parse(parts[2]);
        }
        else
        {
            ChainId = 0;
            PoolId = 0;
        }
    }

    public ValidationResult ValidationResult { get; }
    public long PoolId { get; }
    public long ChainId { get; }
    public bool IsMetadataRequest { get; }
    public bool IsFaviconRequest { get; }

    internal static bool IsMetadataPath(string? path) =>
        string.Equals(path.GetSegment(0), MetadataSegmentName, StringComparison.OrdinalIgnoreCase);

    internal static bool IsFaviconPath(string? path) =>
        string.Equals(path, FaviconPath, StringComparison.OrdinalIgnoreCase);
}