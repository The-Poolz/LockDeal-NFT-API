using System.Net;

namespace MetaDataAPI.Models;

public class HealthCheckResponse() : LambdaResponse(
    body: string.Empty,
    statusCode: HttpStatusCode.OK,
    contentType: ContentType.TextPlain
);