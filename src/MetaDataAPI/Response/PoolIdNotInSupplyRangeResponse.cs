using System.Net;
using System.Numerics;

namespace MetaDataAPI.Response;

public class PoolIdNotInSupplyRangeResponse : LambdaResponse
{
    public PoolIdNotInSupplyRangeResponse(BigInteger poolId)
        : base(
            body: $"Pool ID '{poolId}' must be less then total supply.",
            statusCode: HttpStatusCode.BadRequest
        )
    { }
}