using System.Net;
using FluentValidation.Results;

namespace MetaDataAPI.Response;

public abstract class ErrorResponse
{
    public string ErrorMessage { get; }
    public HttpStatusCode StatusCode { get; }

    protected ErrorResponse(string errorMessage, HttpStatusCode statusCode)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }
}

public class ValidationError : ErrorResponse
{
    public ValidationError(ValidationResult validationResult)
        : base(validationResult.ToString($"{Environment.NewLine}"), HttpStatusCode.BadRequest)
    { }
}

public enum ErrorResponses
{
    [ErrorMessage(HttpStatusCode.BadRequest, "The 'poolId' parameter is missing.")]
    MissingPoolId,
    [ErrorMessage(HttpStatusCode.BadRequest, "The 'id' parameter is not a valid BigInteger.")]
    InvalidPoolId,
    [ErrorMessage(HttpStatusCode.UnprocessableEntity, "The 'id' need to be less then total supply.")]
    PoolIdNotInRange,
    [ErrorMessage(HttpStatusCode.Conflict, "Id from metadata needs to be the same as Id from request.")]
    InvalidResponse,
    [ErrorMessage(HttpStatusCode.InternalServerError, "Failed to create provider.")]
    FailedToCreateProvider
}