using System.Net;

namespace MetaDataAPI.Response;

public class ChainNotSupportedResponse(long chainId) : LambdaResponse(
    body: $"Chain ID {chainId} is not supported.",
    statusCode: HttpStatusCode.NotImplemented
);