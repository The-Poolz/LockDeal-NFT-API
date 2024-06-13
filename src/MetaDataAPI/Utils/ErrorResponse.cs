using System.Net;
using MetaDataAPI.Attributes;

namespace MetaDataAPI.Utils;

public enum ErrorResponse
{
    [ErrorMessage(HttpStatusCode.BadRequest, "The 'id' parameter is not a valid BigInteger.")]
    InvalidId,
    [ErrorMessage(HttpStatusCode.BadRequest, "The 'id' parameter is missing.")]
    MissingId,
    [ErrorMessage(HttpStatusCode.UnprocessableEntity, "The 'id' need to be less then total supply.")]
    PoolIdNotInRange,
    [ErrorMessage(HttpStatusCode.Conflict, "Id from metadata needs to be the same as Id from request.")]
    InvalidResponse,
    [ErrorMessage(HttpStatusCode.InternalServerError, "Failed to create provider.")]
    FailedToCreateProvider
}