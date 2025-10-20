using System.Net;
using FluentValidation.Results;

namespace MetaDataAPI.Models.Errors;

public class ValidationErrorResponse(ValidationResult validationResult) : LambdaResponse(
    body: validationResult.ToString($"{Environment.NewLine}"),
    statusCode: HttpStatusCode.BadRequest,
    contentType: ContentType.TextPlain
);