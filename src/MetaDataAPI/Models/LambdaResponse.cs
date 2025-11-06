using System.Net;
using MetaDataAPI.Routing;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Models;

public abstract class LambdaResponse : ApplicationLoadBalancerResponse
{
    private static readonly List<KeyValuePair<string, string>> CorsHeaders =
    [
        new("Access-Control-Allow-Origin", "*"),
        new("Access-Control-Allow-Headers", "Content-Type"),
        new("Access-Control-Allow-Methods", string.Join(',', LambdaRoutes.AllowedMethods))
    ];

    protected LambdaResponse(
        string body,
        HttpStatusCode statusCode,
        ContentType contentType,
        bool isBase64 = false,
        List<KeyValuePair<string, string>>? extraHeaders = null
    )
    {
        StatusCode = (int)statusCode;
        StatusDescription = statusCode.ToString();
        Body = body;
        IsBase64Encoded = isBase64;
        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Content-Type", contentType }
        };
        CorsHeaders.ForEach(item => Headers.Add(item.Key, item.Value));
        extraHeaders?.ForEach(item => Headers.Add(item.Key, item.Value));
    }

    public sealed class ContentType
    {
        public string Value { get; }
        private ContentType(string value) => Value = value;

        public static readonly ContentType Json = new("application/json");
        public static readonly ContentType TextPlain = new("text/plain");
        public static readonly ContentType Icon = new("image/x-icon");

        public override string ToString() => Value;
        public static implicit operator string(ContentType ct) => ct.Value;
    }
}