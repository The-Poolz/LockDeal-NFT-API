using System.Net;

namespace MetaDataAPI.Models.Errors;

public class PoolIdNotInSupplyRangeResponse(long poolId) : LambdaResponse(
    body: $"Pool ID '{poolId}' must be less then total supply.",
    statusCode: HttpStatusCode.BadRequest,
    contentType: ContentType.TextPlain  
);