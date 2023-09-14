using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Storage;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly IRpcCaller rpcCaller;

    public ProviderFactory(IRpcCaller? rpcCaller = null)
    {
        this.rpcCaller = rpcCaller ?? new RpcCaller();
    }
    public Erc20Token GetErc20Token(string address) => new(address, rpcCaller);
    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) =>
        rpcCaller.GetTotalSupply(Environments.LockDealNftAddress) > poolId;
    public Provider Create(BigInteger poolId) => Create(rpcCaller.GetMetadata(poolId));
    private Provider Create(string metadata) => Create(new BasePoolInfo(metadata,this));
    private Provider Create(BasePoolInfo basePoolInfo)
    {    
        var name = rpcCaller.GetName(basePoolInfo.ProviderAddress);
        var objectToInstantiate = $"MetaDataAPI.Providers.{name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (Provider)Activator.CreateInstance(objectType!, args: basePoolInfo)!;
    }
}