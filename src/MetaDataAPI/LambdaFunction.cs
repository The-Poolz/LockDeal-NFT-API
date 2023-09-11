using System.Net;
using System.Numerics;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly ProviderFactory providerFactory;

    public LambdaFunction() : this(new ProviderFactory()) { }
    public LambdaFunction(ProviderFactory providerFactory)
    {
        this.providerFactory = providerFactory;
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        if (!request.QueryStringParameters.TryGetValue("id", out string idParam))
        {
            throw new InvalidOperationException("Invalid request. The 'id' parameter is missing.");
        }

        if (!BigInteger.TryParse(idParam, out BigInteger poolId))
        {
            throw new InvalidOperationException("Invalid request. The 'id' parameter is not a valid BigInteger.");
        }
      
        var provider = providerFactory.Create(poolId);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JObject.FromObject(provider.GetErc721Metadata()).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
