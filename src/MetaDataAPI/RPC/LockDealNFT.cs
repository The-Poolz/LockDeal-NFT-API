using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Providers.PoolsInfo;
using MetaDataAPI.Storage;
using Net.Web3.EthereumWallet;
using Nethereum.Contracts.Standards.ERC20;
using Nethereum.Contracts.ContractHandlers;
using MetaDataAPI.RPC.Models.Functions.Outputs;
using MetaDataAPI.RPC.Models.Functions.Messages;

namespace MetaDataAPI.RPC;

public class LockDealNFT
{
    private readonly IWeb3 web3;
    private readonly ContractHandler contractHandler;
    private readonly ERC20ContractService contractService;

    public string RpcUrl { get; }

    public LockDealNFT()
        : this(
            new Web3(Environments.RpcUrl),
            Environments.LockDealNftAddress
        )
    {
        // TODO: Use environment variables to retrieve ChainId.
        // Receive RpcUrl and ContractAddress from DB, by ChainId.
        RpcUrl = Environments.RpcUrl;
    }
    public LockDealNFT(IWeb3 web3, string contractAddress)
    {
        this.web3 = web3;
        RpcUrl = Environments.RpcUrl;
        contractService = web3.Eth.ERC20.GetContractService(contractAddress);
        contractHandler = web3.Eth.GetContractHandler(contractAddress);
    }

    public virtual List<PoolInfo> GetFullData(BigInteger poolId)
    {
        return contractHandler
            .QueryAsync<GetFullDataMessage, GetFullDataOutput>(new GetFullDataMessage { PoolId = poolId })
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .Select(x => new PoolInfo(x, RpcUrl))
            .ToList();
    }

    public virtual BigInteger PoolIdToCollateralId(EthereumAddress refundProvider, BigInteger poolId)
    {
        return web3.Eth.GetContractHandler(refundProvider)
            .QueryAsync<PoolIdToCollateralIdMessage, PoolIdToCollateralIdOutput>(new PoolIdToCollateralIdMessage { PoolId = poolId })
            .GetAwaiter()
            .GetResult()
            .CollateralId;
    }

    public virtual bool IsPoolIdWithinSupplyRange(BigInteger poolId)
    {
        return GetTotalSupply() > poolId;
    }

    public virtual BigInteger GetTotalSupply()
    {
        return contractService.TotalSupplyQueryAsync()
            .GetAwaiter()
            .GetResult();
    }
}