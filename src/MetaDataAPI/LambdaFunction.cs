using Nethereum.Web3;
using Amazon.Lambda.Core;
using MetaDataAPI.Request;
using MetaDataAPI.Response;
using MetaDataAPI.Providers;
using MetaDataAPI.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChainManager _chainManager;
    private readonly ILockDealNFTService _lockDealNft;

    public LambdaFunction()
        : this(DefaultServiceProvider.Instance)
    { }

    public LambdaFunction(IServiceProvider serviceProvider)
    {
        _serviceProvider = DefaultServiceProvider.Instance;
        _chainManager = serviceProvider.GetRequiredService<IChainManager>();
        _lockDealNft = serviceProvider.GetRequiredService<ILockDealNFTService>();
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

            _lockDealNft.Initialize(new Web3(chainInfo.RpcUrl), chainInfo.LockDealNFT);
            if (!_lockDealNft.IsPoolIdInSupplyRange(request.PoolId))
            {
                return new PoolIdNotInSupplyRangeResponse(request.PoolId);
            }

            var poolsInfo = _lockDealNft.FetchPoolInfo(request.PoolId);

            var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, _serviceProvider);

            var metadata = provider.GetErc721Metadata();

            return new SuccessResponse(metadata);
        }
        catch (Exception exception)
        {
            lambdaContext.Logger.LogError(exception.ToString());
            return new GeneralErrorResponse();
        }
    }
}
