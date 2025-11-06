using System.Net;

namespace MetaDataAPI.Models;

public class FaviconLambdaResponse(byte[] bytes) : LambdaResponse(
    body: Convert.ToBase64String(bytes),
    statusCode: HttpStatusCode.OK,
    contentType: ContentType.Icon,
    isBase64: true
)
{
    private const string FaviconPath = "/opt/favicon.ico";

    public FaviconLambdaResponse() : this(File.ReadAllBytes(FaviconPath)) { }
}