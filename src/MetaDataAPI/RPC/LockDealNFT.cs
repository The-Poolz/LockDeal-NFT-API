using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Storage;
using MetaDataAPI.RPC.Models.PoolInfo;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ERC20;
using MetaDataAPI.RPC.Models.Functions.Outputs;
using MetaDataAPI.RPC.Models.Functions.Messages;

namespace MetaDataAPI.RPC;

public class LockDealNFT
{
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
        RpcUrl = Environments.RpcUrl;
        contractService = web3.Eth.ERC20.GetContractService(contractAddress);
        contractHandler = web3.Eth.GetContractHandler(contractAddress);
    }

    public virtual List<BasePoolInfo> GetFullData(BigInteger poolId)
    {
        return contractHandler
            .QueryAsync<GetFullDataMessage, GetFullDataOutputDTO>()
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .Select(x => new BasePoolInfo(x, RpcUrl))
            .ToList();
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