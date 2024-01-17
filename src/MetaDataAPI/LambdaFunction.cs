using System.Net;
using System.Numerics;
using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.RPC;

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
            return ApiResponseFactory.CreateResponse(ErrorMessages.MissingIdMessage, HttpStatusCode.BadRequest);

        if (!BigInteger.TryParse(idParam, out var poolId))
            return ApiResponseFactory.CreateResponse(ErrorMessages.InvalidIdMessage, HttpStatusCode.BadRequest);

        try
        {
            var lockDealNFT = new LockDealNFT();

            if (!lockDealNFT.IsPoolIdWithinSupplyRange(poolId))
                return ApiResponseFactory.CreateResponse(ErrorMessages.PoolIdNotInRangeMessage, HttpStatusCode.UnprocessableEntity);

            var provider = providerFactory.Create(poolId);

            if (poolId != provider.PoolInfo.PoolId)
                return ApiResponseFactory.CreateResponse(ErrorMessages.InvalidResponseMessage, HttpStatusCode.Conflict);

            return ApiResponseFactory.CreateResponse(provider.GetJsonErc721Metadata(dynamoDb), HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return ApiResponseFactory.CreateResponse(ErrorMessages.FailedToCreateProviderMessage, HttpStatusCode.InternalServerError);
        }
    }
}
