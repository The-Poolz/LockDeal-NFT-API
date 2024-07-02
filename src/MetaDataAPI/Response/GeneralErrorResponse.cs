using System.Net;

namespace MetaDataAPI.Response;

public class GeneralErrorResponse : LambdaResponse
{
    public GeneralErrorResponse()
        : base(
            body: "An error occurred while creating metadata.",
            statusCode: HttpStatusCode.InternalServerError
        )
    { }
}