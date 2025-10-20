using Nethereum.Web3;
using Amazon.Lambda.Core;
using MetaDataAPI.Models;
using MetaDataAPI.Providers;
using MetaDataAPI.Extensions;
using MetaDataAPI.Models.Errors;
using MetaDataAPI.Services.ChainsInfo;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MetaDataAPI;

public class LambdaFunction(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = DefaultServiceProvider.Instance;
    private readonly IChainManager _chainManager = serviceProvider.GetRequiredService<IChainManager>();
    private readonly ILockDealNFTService _lockDealNft = serviceProvider.GetRequiredService<ILockDealNFTService>();

    public LambdaFunction() : this(DefaultServiceProvider.Instance) { }

    public LambdaResponse FunctionHandler(LambdaRequest request, ILambdaContext lambdaContext)
    {
        try
        {
            if (request.HttpMethod == "OPTIONS")
            {
                return new OptionsResponse();
            }

            if (!request.ValidationResult.IsValid)
            {
                return new ValidationErrorResponse(request.ValidationResult);
            }

            if (!_chainManager.TryFetchChainInfo(request.ChainId, out var chainInfo))
            {
                return new ChainNotSupportedResponse(request.ChainId);
            }

            _lockDealNft.Initialize(new Web3(request.ChainId.ToRpcUrl()), chainInfo.LockDealNFT);
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
