using System.Net;

namespace MetaDataAPI.Models;

public class OptionsResponse() : LambdaResponse(
    body: string.Empty,
    statusCode: HttpStatusCode.OK
);