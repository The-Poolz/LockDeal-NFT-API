using Newtonsoft.Json;
using MetaDataAPI.Validation;
using FluentValidation.Results;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Models;

public class LambdaRequest : ApplicationLoadBalancerRequest
{
    public const string GET_METHOD = "GET";
    public const string OPTIONS_METHOD = "OPTIONS";
    public static readonly string[] AllowedMethods = [GET_METHOD, OPTIONS_METHOD];

    [JsonConstructor]
    public LambdaRequest(string httpMethod, string path)
    {
        Path = path;
        HttpMethod = httpMethod;

        ValidationResult = new LambdaRequestValidator().Validate(this);

        if (ValidationResult.IsValid)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
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
}