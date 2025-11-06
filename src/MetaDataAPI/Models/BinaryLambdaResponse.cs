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

    internal static Func<byte[]> BytesProvider { get; set; } = () => File.ReadAllBytes(FaviconPath);

    public FaviconLambdaResponse() : this(BytesProvider()) { }
}