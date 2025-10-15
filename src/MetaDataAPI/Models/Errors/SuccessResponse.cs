using System.Net;
using Newtonsoft.Json;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Models.Errors;

public class SuccessResponse(Erc721Metadata metadata) : LambdaResponse(
    body: JsonConvert.SerializeObject(metadata),
    statusCode: HttpStatusCode.OK
);