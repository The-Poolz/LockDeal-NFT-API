using MetaDataAPI.Utils;
using Amazon.Lambda.Core;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Request;
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

    public APIGatewayProxyResponse FunctionHandler(LambdaRequest request)
    {
        try
        {
            var error = request.PoolIdToProvider(providerFactory, out var provider);
            if (error != null) return error.Value.Create();

            return ApiResponseFactory.CreateResponse(provider!.GetJsonErc721Metadata());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return ErrorResponse.FailedToCreateProvider.Create();
        }
    }
}
