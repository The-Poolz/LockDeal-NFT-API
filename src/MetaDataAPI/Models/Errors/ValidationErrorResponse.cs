using System.Net;
using FluentValidation;

namespace MetaDataAPI.Models.Errors;

public class ValidationErrorResponse(ValidationException validationException) : LambdaResponse(
    body: validationException.ToString(),
    statusCode: HttpStatusCode.BadRequest,
    contentType: ContentType.TextPlain
);