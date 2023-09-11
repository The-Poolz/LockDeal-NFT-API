using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly IRpcCaller rpcCaller;

    public ProviderFactory(IRpcCaller? rpcCaller = null)
    {
        this.rpcCaller = rpcCaller ?? new RpcCaller();
    }

    public Erc20Token GetErc20Token(string address) => new(address, rpcCaller);
    public IProvider Create(BigInteger poolId) => Create(rpcCaller.GetMetadata(poolId));
    private IProvider Create(string metadata) => Create(new BasePoolInfo(metadata,this));
    private IProvider Create(BasePoolInfo basePoolInfo)
    {    
        var name = rpcCaller.GetName(basePoolInfo.ProviderAddress);
        var objectToInstantiate = $"MetaDataAPI.Providers.{name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return (IProvider)Activator.CreateInstance(objectType!, args: basePoolInfo )!;
    }
}