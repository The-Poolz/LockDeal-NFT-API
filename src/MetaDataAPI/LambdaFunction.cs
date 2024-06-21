using System.Numerics;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Validation;
using MetaDataAPI.Services.Erc20;
using Microsoft.Extensions.DependencyInjection;
using MetaDataAPI.Request;
using MetaDataAPI.Response;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly IServiceProvider serviceProvider;
    private readonly IChainManager chainManager;

    public LambdaFunction() // TODO: Implement chain manager which receive ChainInfo from DB.
        : this(DefaultServiceProvider.Instance)
    { }

    public LambdaFunction(IServiceProvider serviceProvider)
    {
        this.serviceProvider = DefaultServiceProvider.Instance;
        chainManager = serviceProvider.GetService<IChainManager>() ?? throw new ArgumentException($"Service '{nameof(IChainManager)}' is required.");
    }

    public LambdaResponse FunctionHandler(LambdaRequest request)
    {
        var requestValidationResult = new APIGatewayProxyRequestValidator().Validate(request);
        if (!requestValidationResult.IsValid)
        {
            return new LambdaResponse(new ValidationError(requestValidationResult));
        }

        var chainInfo = chainManager.FetchChainInfo(request.ChainId!.Value);

        var poolsInfo = AbstractProvider.FetchPoolInfo(request.PoolId!.Value, chainInfo);

        var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, serviceProvider);

        var metadata = provider.GetErc721Metadata();

        Console.WriteLine(JToken.FromObject(metadata));

        return new LambdaResponse(metadata);
    }
}
