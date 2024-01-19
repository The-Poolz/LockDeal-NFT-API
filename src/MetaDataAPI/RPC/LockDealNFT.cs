using Nethereum.Web3;
using System.Numerics;
using Nethereum.Contracts;
using MetaDataAPI.Storage;
using MetaDataAPI.RPC.ABI;
using MetaDataAPI.RPC.Models.DTOs;
using MetaDataAPI.RPC.Models.PoolInfo;
using Nethereum.Contracts.Standards.ERC20;

namespace MetaDataAPI.RPC;

public class LockDealNFT
{
    private readonly Contract contract;
    private readonly ERC20ContractService contractService;

    public string RpcUrl { get; }

    public LockDealNFT()
        : this(
            new Web3(Environments.RpcUrl),
            ABIProvider.GetABI(),
            Environments.LockDealNftAddress
        )
    {
        // TODO: Use environment variables to retrieve VersionName and ChainId.
        // Receive RpcUrl and ContractAddress from DB, by ChainId.
        // Receive ContractABI from API, by VersionName.
        RpcUrl = Environments.RpcUrl;

    }
    public LockDealNFT(IWeb3 web3, string contractABI, string contractAddress)
    {
        contract = web3.Eth.GetContract(contractABI, contractAddress);
        contractService = web3.Eth.ERC20.GetContractService(contractAddress);
        RpcUrl = Environments.RpcUrl;
    }

    public virtual List<BasePoolInfo> GetFullData(BigInteger poolId)
    {
        return contract.GetFunction("getFullData")
            .CallDeserializingToObjectAsync<GetFullDataOutputDTO>(poolId)
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