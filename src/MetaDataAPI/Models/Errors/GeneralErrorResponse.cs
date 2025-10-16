using System.Net;

namespace MetaDataAPI.Models.Errors;

public class GeneralErrorResponse() : LambdaResponse(
    body: "An error occurred while creating metadata.",
    statusCode: HttpStatusCode.InternalServerError
);