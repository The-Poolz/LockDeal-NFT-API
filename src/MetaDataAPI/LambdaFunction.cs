using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly ProviderFactory providerFactory;
    private readonly DynamoDb dynamoDb;

    public LambdaFunction() : this(new ProviderFactory(), new DynamoDb()) { }
    public LambdaFunction(ProviderFactory providerFactory, DynamoDb dynamoDb)
    {
        this.providerFactory = providerFactory;
        this.dynamoDb = dynamoDb;
    }

    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request)
    {
        if (!request.QueryStringParameters.TryGetValue("id", out var idParam))
            return ApiResponseFactory.CreateResponse(ErrorMessages.missingIdMessage, HttpStatusCode.BadRequest);

        if (!BigInteger.TryParse(idParam, out var poolId))
            return ApiResponseFactory.CreateResponse(ErrorMessages.invalidIdMessage, HttpStatusCode.BadRequest);

        IProvider provider;
        try
        {
            if (!providerFactory.IsPoolIdWithinSupplyRange(poolId))
                return ApiResponseFactory.CreateResponse(ErrorMessages.poolIdNowInRangeMessage, HttpStatusCode.UnprocessableEntity);

            provider = providerFactory.Create(poolId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return ApiResponseFactory.CreateResponse(ErrorMessages.failedToCreateProviderMessage, HttpStatusCode.InternalServerError);
        }

        if (poolId != provider.PoolInfo.PoolId)
            return ApiResponseFactory.CreateResponse(ErrorMessages.invalidResponseMessage, HttpStatusCode.Conflict);

        return ApiResponseFactory.CreateResponse(provider.GetJsonErc721Metadata(dynamoDb), HttpStatusCode.OK);
    }
}
