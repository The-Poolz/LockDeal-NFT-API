using MediatR;
using MetaDataAPI.Models;
using MetaDataAPI.Providers;
using MetaDataAPI.Extensions;
using MetaDataAPI.Models.Errors;
using MetaDataAPI.Services.Http;
using MetaDataAPI.Services.ChainsInfo;
using Poolz.Finance.CSharp.Polly.Extensions;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI.Routing.Requests;

public class GetMetadataRequestHandler(
    IServiceProvider serviceProvider,
    IChainManager chainManager,
    ILockDealNFTService lockDealNft,
    IWeb3Factory web3Factory,
    IRetryExecutor retry
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

        lockDealNft.Initialize(web3Factory.Create(chainId.ToRpcUrl()), chainInfo.LockDealNFT);
        var isPoolIdInSupplyRange = retry.Execute(_ => lockDealNft.IsPoolIdInSupplyRange(poolId), ct: cancellationToken);

        if (!isPoolIdInSupplyRange)
        {
            return Task.FromResult<LambdaResponse>(new PoolIdNotInSupplyRangeResponse(chainId));
        }

        var poolsInfo = lockDealNft.FetchPoolInfo(poolId);
        var provider = AbstractProvider.CreateFromPoolInfo(poolsInfo, chainInfo, serviceProvider);
        var metadata = provider.GetErc721Metadata();

        return Task.FromResult<LambdaResponse>(new SuccessResponse(metadata));
    }
}