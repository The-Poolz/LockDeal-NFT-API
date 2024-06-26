using System.Net;
using System.Numerics;

namespace MetaDataAPI.Response;

public class ChainNotSupportedResponse : LambdaResponse
{
    public ChainNotSupportedResponse(BigInteger chainId)
        : base(
            body: $"Chain ID {chainId} is not supported.",
            statusCode: HttpStatusCode.NotImplemented
        )
    { }
}