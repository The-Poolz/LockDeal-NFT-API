using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using MetaDataAPI.Storage;
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
        if (!request.QueryStringParameters.TryGetValue("id", out var idParam))
            return ApiResponseFactory.MissingId();

        if (!BigInteger.TryParse(idParam, out var poolId))
            return ApiResponseFactory.InvalidId();

        try
        {
            if (!providerFactory.IsPoolIdWithinSupplyRange(poolId))
                return ApiResponseFactory.PoolIdNotInRange();

            var provider = providerFactory.Create(poolId);

            if (poolId != provider.PoolInfo.PoolId)
                return ApiResponseFactory.InvalidResponse();

            return ApiResponseFactory.CreateResponse(provider.GetJsonErc721Metadata(), HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return ApiResponseFactory.FailedToCreateProvider();
        }
    }
}
