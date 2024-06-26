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
    private readonly IServiceProvider _serviceProvider;
    private readonly IChainManager _chainManager;

    public LambdaFunction()
        : this(DefaultServiceProvider.Instance)
    { }

    public LambdaFunction(IServiceProvider serviceProvider)
    {
        _serviceProvider = DefaultServiceProvider.Instance;
        _chainManager = serviceProvider.GetService<IChainManager>() ?? throw new ArgumentException($"Service '{nameof(IChainManager)}' is required.");
    }

    public LambdaResponse FunctionHandler(LambdaRequest request, ILambdaContext lambdaContext)
    {
        try
        {
            if (request.ValidationResult != null)
            {
                return new ValidationErrorResponse(request.ValidationResult);
            }

            if (!_chainManager.TryFetchChainInfo(request.ChainId, out var chainInfo))
            {
                return new ChainNotSupportedResponse(request.ChainId);
            }

            var poolsInfo = AbstractProvider.FetchPoolInfo(request.PoolId, chainInfo);

            var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, _serviceProvider);

            var metadata = provider.GetErc721Metadata();

            Console.WriteLine(JToken.FromObject(metadata)); // TODO: Remove before merge PR

            return new SuccessResponse(metadata);
        }
        catch (Exception exception)
        {
            lambdaContext.Logger.LogError(exception.ToString());
            return new GeneralErrorResponse();
        }
    }
}
