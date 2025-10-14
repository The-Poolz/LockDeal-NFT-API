using System.Net;

namespace MetaDataAPI.Models.Errors;

public class ChainNotSupportedResponse(long chainId) : LambdaResponse(
    body: $"Chain ID {chainId} is not supported.",
    statusCode: HttpStatusCode.NotImplemented
);