using Nethereum.Web3;
using System.Numerics;
using Nethereum.Contracts;
using MetaDataAPI.RPC.Models.DTOs;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.RPC;

public class LockDealNFT
{
    private readonly Contract contract;

    public LockDealNFT(string versionName, BigInteger chainId)
    {
        // TODO: Receive RpcUrl and ContractAddress from DB, by ChainId
        // TODO: Receive ContractABI from API, by VersionName
    }

    public LockDealNFT(IWeb3 web3, string contractABI, string contractAddress)
    {
        contract = web3.Eth.GetContract(contractABI, contractAddress);
    }

    public LockDealNFT(Contract contract)
    {
        this.contract = contract;
    }

    public virtual IEnumerable<BasePoolInfo> GetFullData(BigInteger poolId)
    {
        return contract.GetFunction("getFullData")
            .CallDeserializingToObjectAsync<GetFullDataOutputDTO>(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .Select(x => new BasePoolInfo(x));
    }
}