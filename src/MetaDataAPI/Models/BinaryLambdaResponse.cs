using System.Net;

namespace MetaDataAPI.Models;

public class FaviconLambdaResponse : LambdaResponse
{
    private const string FaviconPath = "/opt/favicon.ico";

    public FaviconLambdaResponse() : this(File.ReadAllBytes(FaviconPath)) { }

    public FaviconLambdaResponse(byte[] bytes) : base(Convert.ToBase64String(bytes), HttpStatusCode.OK)
    {
        IsBase64Encoded = true;
        Headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "image/x-icon"
        };
    }
}