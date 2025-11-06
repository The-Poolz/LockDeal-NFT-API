using MediatR;
using Nethereum.Web3;
using MetaDataAPI.Models;
using MetaDataAPI.Providers;
using MetaDataAPI.Extensions;
using MetaDataAPI.Models.Errors;
using MetaDataAPI.Services.ChainsInfo;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI.Routing.Requests;

public class GetMetadataRequestHandler(
    IServiceProvider serviceProvider,
    IChainManager chainManager,
    ILockDealNFTService lockDealNft
) : IRequestHandler<GetMetadataRequest, LambdaResponse>
{
    public Task<LambdaResponse> Handle(GetMetadataRequest request, CancellationToken cancellationToken)
    {
        var pathSegments = request.Path.SplitPath();
        var chainId = long.Parse(pathSegments[1]);
        var poolId = long.Parse(pathSegments[2]);

        if (!chainManager.TryFetchChainInfo(chainId, out var chainInfo))
        {
            return Task.FromResult<LambdaResponse>(new ChainNotSupportedResponse(chainId));
        }

        lockDealNft.Initialize(new Web3(chainId.ToRpcUrl()), chainInfo.LockDealNFT);
        if (!lockDealNft.IsPoolIdInSupplyRange(poolId))
        {
            return Task.FromResult<LambdaResponse>(new PoolIdNotInSupplyRangeResponse(poolId));
        }

        var poolsInfo = lockDealNft.FetchPoolInfo(poolId);
        var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, serviceProvider);
        var metadata = provider.GetErc721Metadata();

        return Task.FromResult<LambdaResponse>(new SuccessResponse(metadata));
    }
}