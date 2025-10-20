using System.Net;

namespace MetaDataAPI.Models.Errors;

public class OptionsResponse() : LambdaResponse(
    body: string.Empty,
    statusCode: HttpStatusCode.NoContent
);
