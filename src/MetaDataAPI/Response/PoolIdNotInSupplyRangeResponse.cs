using System.Net;

namespace MetaDataAPI.Response;

public class PoolIdNotInSupplyRangeResponse(long poolId) : LambdaResponse(
    body: $"Pool ID '{poolId}' must be less then total supply.",
    statusCode: HttpStatusCode.BadRequest
);