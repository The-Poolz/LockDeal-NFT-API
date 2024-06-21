using System.Net;
using Newtonsoft.Json;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Response;

public class SuccessResponse : LambdaResponse
{
    public SuccessResponse(Erc721Metadata metadata)
        : base(
            body: JsonConvert.SerializeObject(metadata),
            statusCode: HttpStatusCode.OK
        )
    { }
}