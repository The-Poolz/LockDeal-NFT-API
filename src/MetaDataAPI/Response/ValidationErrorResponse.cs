using System.Net;
using FluentValidation.Results;

namespace MetaDataAPI.Response;

public class ValidationErrorResponse : LambdaResponse
{
    public ValidationErrorResponse(ValidationResult validationResult)
        : base(
            body: validationResult.ToString($"{Environment.NewLine}"),
            statusCode: HttpStatusCode.BadRequest
        )
    { }
}