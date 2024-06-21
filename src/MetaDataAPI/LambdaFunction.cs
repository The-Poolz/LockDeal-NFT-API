using Amazon.Lambda.Core;
using MetaDataAPI.Request;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Response;
using MetaDataAPI.Providers;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;

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
        if (request.ValidationResult != null)
        {
            return new LambdaResponse(new ValidationError(request.ValidationResult));
        }

        var chainInfo = chainManager.FetchChainInfo(request.ChainId);

        var poolsInfo = AbstractProvider.FetchPoolInfo(request.PoolId, chainInfo);

        var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, serviceProvider);

        var metadata = provider.GetErc721Metadata();

        Console.WriteLine(JToken.FromObject(metadata));

        return new LambdaResponse(metadata);
    }
}
